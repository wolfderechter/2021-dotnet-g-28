using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository,IWebHostEnvironment hostingEnvironment,ISupportManagerRepository supportManagerRepository,ICompanyRepository companyRepository)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _hostingEnvironment = hostingEnvironment;
            _supportManagerRepository = supportManagerRepository;
            _companyRepository = companyRepository;
        }
  
        public async Task<IActionResult> Index()
        {
            TicketIndexViewModel model = new TicketIndexViewModel();
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
                ViewData["NotificationCount"] = contactPerson.Notifications.Where(n=>!n.IsRead).Count();
            }
           
            model.CheckBoxItems = new List<StatusModelTicket>();
            foreach (TicketEnum.status ticketStatus in Enum.GetValues(typeof(TicketEnum.status)))
            {
                model.CheckBoxItems.Add(new StatusModelTicket() { Status = ticketStatus, IsSelected = false });
            }
            model.CheckBoxItems.SingleOrDefault(c => c.Status == TicketEnum.status.Created).IsSelected = true;
            model.CheckBoxItems.SingleOrDefault(c => c.Status == TicketEnum.status.InProgress).IsSelected = true;
            //insert list of duurcheckbox items into model
            model.Tickets = _ticketRepository.GetByStatus(new List<TicketEnum.status> { TicketEnum.status.Created, TicketEnum.status.InProgress });

            return View(model);
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
                    string uniqueFileName = null;
                    if (ticketEditViewModel.Picture != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "bijlagen");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + ticketEditViewModel.Picture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        ticketEditViewModel.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
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
                    string uniqueFileName = null;
                    if (ticketEditViewModel.Picture != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "bijlagen");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + ticketEditViewModel.Picture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        ticketEditViewModel.Picture.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    Ticket ticket = _ticketRepository.GetBy(ticketNr);
                    ticket.EditTicket(ticketEditViewModel.Title, ticketEditViewModel.Remark, ticketEditViewModel.Description, ticketEditViewModel.Type, uniqueFileName);
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

        public IActionResult Stop(int ticketNr)
        {
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ticket.Status = TicketEnum.status.Cancelled;
            _ticketRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddReaction(int ticketNr, string reaction,TicketIndexViewModel model) 
        {
            var user = await _userManager.GetUserAsync(User);
            ContactPerson contact = _contactPersonRepository.getById(user.Id);
            Ticket ticket = _ticketRepository.GetBy(ticketNr);
            if (ticket == null)
                return NotFound();
            ticket.AddReaction(new Reaction(reaction, contact.FirstName + " " + contact.LastName,false, ticketNr));
            _ticketRepository.SaveChanges();
            TempData["message"] = $"Your reaction has been succesfully added";
            return RedirectToAction(nameof(Index),model);
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
