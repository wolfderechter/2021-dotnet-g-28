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
    [Authorize(Policy = "Customer")]
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHtmlLocalizer<TicketController> _localizor;

        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository, IWebHostEnvironment hostingEnvironment)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //get signed in user
            var user = await _userManager.GetUserAsync(User);

            //get contactperson matching with signed in user
            ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);

            //model initialiseren
            TicketIndexViewModel model = new TicketIndexViewModel
            {
                Tickets = _ticketRepository.GetByContactPersonId(contactPerson.Id)
            };

            //Status filteren
            model.CheckBoxItemsStatus = new List<StatusModelTicket>();
            foreach (TicketEnum.status ticketStatus in Enum.GetValues(typeof(TicketEnum.status)))
            {
                model.CheckBoxItemsStatus.Add(new StatusModelTicket() { Status = ticketStatus, IsSelected = false });
            }
            model.CheckBoxItemsStatus.SingleOrDefault(c => c.Status == TicketEnum.status.Created).IsSelected = true;
            model.CheckBoxItemsStatus.SingleOrDefault(c => c.Status == TicketEnum.status.InProgress).IsSelected = true;

            //Type filteren
            model.CheckBoxItemsType = new List<TypeModelTicket>();
            foreach (TicketEnum.type ticketType in Enum.GetValues(typeof(TicketEnum.type)))
            {
                model.CheckBoxItemsType.Add(new TypeModelTicket() { Type = ticketType, IsSelected = false });
            }
            model.CheckBoxItemsType.SingleOrDefault(c => c.Type == TicketEnum.type.ProductionStopped).IsSelected = true;
            model.CheckBoxItemsType.SingleOrDefault(c => c.Type == TicketEnum.type.ProductionWillStop).IsSelected = true;

            //insert list of duurcheckbox items into model
            model.Tickets = _ticketRepository.GetByStatusAndType(new List<TicketEnum.status> { TicketEnum.status.Created, TicketEnum.status.InProgress }, new List<TicketEnum.type> { TicketEnum.type.ProductionStopped, TicketEnum.type.ProductionWillStop });

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(TicketIndexViewModel model)
        {
            //getting contactperson from the signedin user
            ContactPerson contactPerson = await GetLoggedInContactPerson();
            //getting the selected statusses/
            List<TicketEnum.status> selectedStatusses = model.CheckBoxItemsStatus.Where(c => c.IsSelected).Select(c => c.Status).ToList();
            List<TicketEnum.type> selectedTypes = model.CheckBoxItemsType.Where(c => c.IsSelected).Select(c => c.Type).ToList();
            //getting contracts connected to statusses
            model.Tickets = _ticketRepository.GetByStatusAndType(selectedStatusses, selectedTypes);
            return View(model);
        }

        private async Task<ContactPerson> GetLoggedInContactPerson()
        {
            var user = await _userManager.GetUserAsync(User);
            return _contactPersonRepository.getById(user.Id);
        }

        public IActionResult Create()
        {
            ViewData["IsEdit"] = false;
            ViewData["typeTickets"] = TypeTickets();
            //return View();
            return View(nameof(Edit), new TicketEditViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TicketEditViewModel ticketEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    ContactPerson contact = _contactPersonRepository.getById(user.Id);
                    var ticket = new Ticket(DateTime.Now, ticketEditViewModel.Title, ticketEditViewModel.Description, ticketEditViewModel.Type, TicketEnum.status.Created);

                    if (ticketEditViewModel.Attachments != null)
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

                    _ticketRepository.Add(ticket);
                    contact.AddTicket(ticket);
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
                //return View(nameof(Index), ticketEditViewModel);

            }


        }
        private SelectList TypeTickets()
        {
            var typeTickets = new List<TicketEnum.type>();
            foreach (TicketEnum.type typeTicket in Enum.GetValues(typeof(TicketEnum.type)))
            {
                typeTickets.Add(typeTicket);
            }
            return new SelectList(typeTickets);
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

        public IActionResult Download(TicketEditViewModel ticketEditViewModel, string filename) {
            //byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "attachments", filename));
            var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", filename);
            //var vpath = filepath.Replace(filepath, "~/attachments").Replace(@"\", "/");
            //return File(vpath, "image/jpg", filename);

            return PhysicalFile(filepath, MimeTypes.GetMimeType(filepath), filename);

        }
      

        [HttpPost]
        public IAction SetLanguage2(string culture)
        {
           //string language = (string)filterContext.RouteData.Values["language"] ?? "nl";
          //  string culture = (string)filterContext.RouteData.Values["culture"] ?? "NL";

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}",  "nl","NL"));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "nl", "NL"));
            return RedirectToAction(nameof(Index));
        }
    }
}
