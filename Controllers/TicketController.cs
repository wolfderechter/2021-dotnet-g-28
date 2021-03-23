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
using Newtonsoft.Json;


namespace _2021_dotnet_g_28.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        #region variables
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISupportManagerRepository _supportManagerRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHtmlLocalizer<TicketController> _localizor;
        #endregion


        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository, IWebHostEnvironment hostingEnvironment, ISupportManagerRepository supportManagerRepository, ICompanyRepository companyRepository)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _hostingEnvironment = hostingEnvironment;
            _supportManagerRepository = supportManagerRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(TicketIndexViewModel model)
        {
            //if tempdata is filled with a model -> use this model (gets emptied when read)
            if (TempData["tempModel"] != null)
            {
                model = JsonConvert.DeserializeObject<TicketIndexViewModel>((string)TempData["tempModel"]);
            }

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
                ViewData["Notifications"] = contactPerson.Notifications.Where(n => !n.IsRead).ToList();
            }

            //only do this when index gets called for first time
            if (model.CheckBoxItemsStatus == null)
            {
                //Filteren op STATUS
                //populate filter with checkbox options and setting 2 selected
                model.CheckBoxItemsStatus = new List<StatusModelTicket>();
                foreach (TicketEnum.Status ticketStatus in Enum.GetValues(typeof(TicketEnum.Status)))
                {
                    model.CheckBoxItemsStatus.Add(new StatusModelTicket() { Status = ticketStatus, IsSelected = false });
                }
                model.CheckBoxItemsStatus.SingleOrDefault(c => c.Status == TicketEnum.Status.Created).IsSelected = true;
                model.CheckBoxItemsStatus.SingleOrDefault(c => c.Status == TicketEnum.Status.InProgress).IsSelected = true;

                //Filteren op TYPE
                model.CheckBoxItemsType = new List<TypeModelTicket>();
                foreach (TicketEnum.Type ticketType in Enum.GetValues(typeof(TicketEnum.Type)))
                {
                    model.CheckBoxItemsType.Add(new TypeModelTicket() { Type = ticketType, IsSelected = false });
                }
                model.CheckBoxItemsType.SingleOrDefault(c => c.Type == TicketEnum.Type.ProductionStopped).IsSelected = true;
                model.CheckBoxItemsType.SingleOrDefault(c => c.Type == TicketEnum.Type.ProductionWillStop).IsSelected = true;


                //insert tickets with status created,  in progress and type production stopped and production will stop
                model.Tickets = _ticketRepository.GetByStatusAndType(new List<TicketEnum.Status> { TicketEnum.Status.Created, TicketEnum.Status.InProgress },
                    new List<TicketEnum.Type> { TicketEnum.Type.ProductionStopped, TicketEnum.Type.ProductionWillStop });
            }
            else
            {
                //reload tickets with new selected status
                List<TicketEnum.Status> statusList = new List<TicketEnum.Status>();
                foreach (var item in model.CheckBoxItemsStatus)
                {
                    if (item.IsSelected)
                    {
                        statusList.Add(item.Status);
                    }
                }


                //reload tickets with new selected type
                List<TicketEnum.Type> typeList = new List<TicketEnum.Type>();
                //foreach (var item in model.CheckBoxItemsType)
                //{
                //    if (item.IsSelected)
                //    {
                //        typeList.Add(item.Type);
                //    }
                //}
                model.Tickets = _ticketRepository.GetByStatusAndType(statusList, typeList);
            }

            if (TempData["openTicket"] != null)
            {
                model.OpenedTicket = _ticketRepository.GetBy((int)TempData["openTicket"]);
            }

            //writes model to session so that in next request it can get read and put into tempdata
            WriteTicketIndexViewModelToSession(model);

            return View(model);
        }

        //[HttpPost]
        //public async Task<ActionResult> Index(TicketIndexViewModel model)
        //{
        //    //getting contactperson from the signedin user
        //    ContactPerson contactPerson = await GetLoggedInContactPerson();
        //    //getting the selected statusses/
        //    List<TicketEnum.Status> selectedStatusses = model.CheckBoxItemsStatus.Where(c => c.IsSelected).Select(c => c.Status).ToList();

        //    //getting the selected types/
        //    List<TicketEnum.Type> selectedTypes = model.CheckBoxItemsType.Where(c => c.IsSelected).Select(c => c.Type).ToList();

        //    //getting contracts connected to statusses
        //    model.Tickets = _ticketRepository.GetByStatusAndType(selectedStatusses, selectedTypes);
        //    return View(model);
        //}

        public async Task<IActionResult> Create()
        {
            GetTicketIndexViewModelFromSessionAndPutInTempData();
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
                    var ticket = new Ticket(DateTime.Now, ticketEditViewModel.Title, ticketEditViewModel.Description, ticketEditViewModel.Type, TicketEnum.Status.Created);


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
                                ViewData["errorMessageType"] = "Acceptable filetypes is pdf/png/jpg/jpeg/";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //amount of files controle
                            if (ticketEditViewModel.Attachments.Count() > 5)
                            {
                                ViewData["errorMessageAmount"] = "Max amount of attachments is 5!";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //file size controleren
                            if (attachm.Length > 5242880)
                            {
                                ViewData["errorMessageSize"] = "Max filesize is 5MB!";
                                ViewData["IsEdit"] = false;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            attachm.CopyTo(stream);
                            ticket.Attachments.Add(uniqueFileName);

                        }
                    }
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

        public IActionResult Edit(int ticketNr)
        {
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ViewData["IsEdit"] = true;
            ViewData["typeTickets"] = TypeTickets();
            return View(new TicketEditViewModel(ticket));
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
                                ViewData["errorMessageType"] = "Acceptable filetypes is pdf/png/jpg/jpeg/";
                                ViewData["IsEdit"] = true;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //amount of files controle
                            if (ticketEditViewModel.Attachments.Count() > 5)
                            {
                                ViewData["errorMessageAmount"] = "Max amount of attachments is 5!";
                                ViewData["IsEdit"] = true;
                                return View(nameof(Edit), ticketEditViewModel);

                            }
                            //file size controleren
                            if (attachm.Length > 5242880)
                            {
                                ViewData["errorMessageSize"] = "Max filesize is 5MB!";
                                ViewData["IsEdit"] = true;
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

        public async Task<IActionResult> AddReaction(int ticketNr, string reaction, TicketIndexViewModel model)
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

        public IActionResult Download(TicketEditViewModel ticketEditViewModel, string filename)
        {
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
                            _companyRepository.GetAll().OrderBy(c => c.CompanyName),
                            nameof(Company.CompanyNr),
                            nameof(Company.CompanyName));
        }

        private async Task<ContactPerson> GetLoggedInContactPerson()
        {
            var user = await _userManager.GetUserAsync(User);
            return _contactPersonRepository.getById(user.Id);
        }

        //this method esures that our filter doesn't reset each time we leave index
        //model gets saved in session when we leave index -> see index
        //in this method session gets read and cleared and put the model in tempdata -> tempdata gets checked at the start of index
        //this method gets called each time when our next request will be index
        public void GetTicketIndexViewModelFromSessionAndPutInTempData()
        {
            var model = ReadTicketIndexViewModelFromSession();
            HttpContext.Session.Clear();
            TempData["tempModel"] = JsonConvert.SerializeObject(model);
        }

        //this method writes a ticketindexviewmodel into session
        private void WriteTicketIndexViewModelToSession(TicketIndexViewModel model)
        {
            HttpContext.Session.SetString("model", JsonConvert.SerializeObject(model));
        }

        //this method reads and returns a ticketindexviewmodel from session
        private TicketIndexViewModel ReadTicketIndexViewModelFromSession()
        {
            return JsonConvert.DeserializeObject<TicketIndexViewModel>(HttpContext.Session.GetString("model"));
        }
    }
}
