using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Localization;
using System.Threading;
using System.Globalization;

namespace _2021_dotnet_g_28.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISupportManagerRepository _supportManagerRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHtmlLocalizer<TicketController> _localizor;

        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository,IWebHostEnvironment hostingEnvironment,ISupportManagerRepository supportManagerRepository,ICompanyRepository companyRepository)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _hostingEnvironment = hostingEnvironment;
            _supportManagerRepository = supportManagerRepository;
            _companyRepository = companyRepository;
        }
  
        public async Task<IActionResult> Index(TicketIndexViewModel model)
        {
            //TicketIndexViewModel model = new TicketIndexViewModel();
            //get signed in user
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SupportManager"))
            {
                //get support manager connected 
                var supManager = _supportManagerRepository.GetById(user.Id);
                ViewData["GebruikersNaam"] = supManager.FirstName + " " + supManager.LastName;
                model.Tickets = _ticketRepository.GetAll();
            }
            else 
            {
                //get contactperson matching with signed in user
                ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);
                ViewData["GebruikersNaam"] = contactPerson.FirstName + " " + contactPerson.LastName;
                model.Tickets = _ticketRepository.GetByContactPersonId(contactPerson.Id);
                ViewData["Notifications"] = contactPerson.Notifications.Where(n=>!n.IsRead).ToList();
            }
           
            //only do this when index gets called for first time
            if(model.CheckBoxItems == null)
            {
                //populate filter with checkbox options and setting 2 selected
                model.CheckBoxItems = new List<StatusModelTicket>();
                foreach (TicketEnum.Status ticketStatus in Enum.GetValues(typeof(TicketEnum.Status)))
                {
                    model.CheckBoxItems.Add(new StatusModelTicket() { Status = ticketStatus, IsSelected = false });
                }
                model.CheckBoxItems.SingleOrDefault(c => c.Status == TicketEnum.Status.Created).IsSelected = true;
                model.CheckBoxItems.SingleOrDefault(c => c.Status == TicketEnum.Status.InProgress).IsSelected = true;

                //insert tickets with status created and in progress
                model.Tickets = _ticketRepository.GetByStatus(new List<TicketEnum.Status> { TicketEnum.Status.Created, TicketEnum.Status.InProgress });
            } else
            {          
                //reload tickets with new selected status
                List<TicketEnum.Status> statusList = new List<TicketEnum.Status>();
                foreach (var item in model.CheckBoxItems)
                {
                    if (item.IsSelected)
                    {
                        statusList.Add(item.Status);
                    }
                }
                model.Tickets = _ticketRepository.GetByStatus(statusList);
            }

        [HttpPost]
        public async Task<ActionResult> Index(TicketIndexViewModel model)
        {
            //getting contactperson from the signedin user
            ContactPerson contactPerson = await GetLoggedInContactPerson();
            //getting the selected statusses/
            List<TicketEnum.status> selectedStatusses = model.CheckBoxItems.Where(c => c.IsSelected).Select(c => c.Status).ToList();
           
            //getting contracts connected to statusses
            model.Tickets = _ticketRepository.GetByStatus(selectedStatusses);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["IsEdit"] = false;
            ViewData["typeTickets"] = TypeTickets();
            if (User.IsInRole("SupportManager"))
            {
                //get support manager connected 
                var user = await _userManager.GetUserAsync(User);
                var supManager = _supportManagerRepository.GetById(user.Id);
                ViewData["Customers"] = GetSelectListCompanies();
            }
            return View(nameof(Edit), new TicketEditViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TicketEditViewModel ticketEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //getting company from contactperson or from 
                    Company company;
                    if (!User.IsInRole("SupportManager"))
                    {
                        var user = await _userManager.GetUserAsync(User);
                        ContactPerson contact = _contactPersonRepository.getById(user.Id);
                        company = contact.Company;
                    }
                    else 
                    {
                        company = _companyRepository.GetByNr(ticketEditViewModel.CompanyNr);
                    }
                    if (ticketEditViewModel.Attachments != null)

                    var ticket = new Ticket(DateTime.Now, ticketEditViewModel.Title, ticketEditViewModel.Description, ticketEditViewModel.Type, TicketEnum.status.Created);
                    {
                        ticket.Attachments = new List<string>();
                        foreach(IFormFile attachm in ticketEditViewModel.Attachments)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + attachm.FileName;
                            var path = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", uniqueFileName);
                            var stream = new FileStream(path, FileMode.Create);
                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "jpg", "jpeg", "png" };
                            //filetypes controle
                            if (!supportedTypes.Contains(attachm.FileName.Substring(attachm.FileName.LastIndexOf(".") + 1)))
                            {
                                TempData["errorMessageType"] = "Acceptable filetypes is pdf/png/jpg/jpeg/";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //amount of files controle
                            if (ticketEditViewModel.Attachments.Count() > 5)
                            {
                                TempData["errorMessageAmount"] = "Max amount of attachments is 5!";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //file size controleren
                            if (attachm.Length > 5242880)
                            {
                                TempData["errorMessageSize"] = "Max filesize is 5MB!";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            attachm.CopyTo(stream);
                            ticket.Attachments.Add(uniqueFileName);
                            
                        }
                    }
                    var ticket = new Ticket(DateTime.Now, ticketEditViewModel.Title, ticketEditViewModel.Remark, ticketEditViewModel.Description, ticketEditViewModel.Type, TicketEnum.status.Created,uniqueFileName);
                    _ticketRepository.Add(ticket);
                    company.AddTicket(ticket);
                    _ticketRepository.SaveChanges();
                    TempData["message"] = $"You successfully created a ticket.";
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, the ticket was not created...";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nameof(Edit), ticketEditViewModel);
            }


        }
        
        [HttpPost]
        public IActionResult Edit(int ticketNr, TicketEditViewModel ticketEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Ticket ticket = _ticketRepository.GetBy(ticketNr);

                    if (ticketEditViewModel.Attachments != null)
                    {
                        ticket.Attachments = new List<string>();
                        foreach (IFormFile attachm in ticketEditViewModel.Attachments)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + attachm.FileName;
                            var path = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", uniqueFileName);
                            var stream = new FileStream(path, FileMode.Create);
                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "jpg", "jpeg", "png" };
                            //filetypes controle
                            if (!supportedTypes.Contains(attachm.FileName.Substring(attachm.FileName.LastIndexOf(".") + 1)))
                            {
                                TempData["errorMessageType"] = "Acceptable filetypes is pdf/png/jpg/jpeg/";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //amount of files controle
                            if (ticketEditViewModel.Attachments.Count() > 5)
                            {
                                TempData["errorMessageAmount"] = "Max amount of attachments is 5!";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //file size controleren
                            if (attachm.Length > 5242880)
                            {
                                TempData["errorMessageSize"] = "Max filesize is 5MB!";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            attachm.CopyTo(stream);
                            ticket.Attachments.Add(uniqueFileName);
                        }
                    }

                    ticket.EditTicket(ticketEditViewModel.Title, ticketEditViewModel.Description, ticketEditViewModel.Type);
                    _ticketRepository.SaveChanges();
                    TempData["message"] = $"You successfully updated the ticket.";
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, ticket was not updated...";
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nameof(Edit), ticketEditViewModel);
            }

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Ticket ticket = _ticketRepository.GetBy(id);
                if (ticket == null)
                    return NotFound();

                _ticketRepository.Delete(ticket);
                _ticketRepository.SaveChanges();
                TempData["message"] = $"Ticket {id} was sucessfully cancelled…";
                //return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["error"] = $"Sorry, something went wrong, Ticket {id} was not cancelled…";
            }
            return Ok();
        }

        public async Task<IActionResult> AddReaction(int ticketNr, string reaction,TicketIndexViewModel model) 
        {
            var user = await _userManager.GetUserAsync(User);
            ContactPerson contact = _contactPersonRepository.getById(user.Id);
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ticket.AddReaction(new Reaction(reaction, contact.FirstName + " " + contact.LastName, false, ticketNr));
            _ticketRepository.SaveChanges();
            TempData["message"] = $"Your reaction has been succesfully added";
            return RedirectToAction(nameof(Index), model);
        }

        public IActionResult Download(TicketEditViewModel ticketEditViewModel, string filename) {
            //byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "attachments", filename));
            //var vpath = filepath.Replace(filepath, "~/attachments").Replace(@"\", "/");
            var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", filename);
            //return File(vpath, "image/jpg", filename);
            return PhysicalFile(filepath, MimeTypes.GetMimeType(filepath), filename);


        }
        private SelectList TypeTickets()
        {
            var typeTickets = new List<TicketEnum.Type>();
            foreach (TicketEnum.Type typeTicket in Enum.GetValues(typeof(TicketEnum.Type)))
            {
                typeTickets.Add(typeTicket);
            }
            return new SelectList(typeTickets);
        }
        private SelectList GetSelectListCompanies()
        {
            return new SelectList(
                            _companyRepository.GetAll().OrderBy(c=>c.CompanyName),
                            nameof(Company.CompanyNr),
                            nameof(Company.CompanyName));
        }

        private async Task<ContactPerson> GetLoggedInContactPerson()
        {
            var user = await _userManager.GetUserAsync(User);
            return _contactPersonRepository.getById(user.Id);
        }
    }

}
