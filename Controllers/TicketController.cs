using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2021_dotnet_g_28.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository,IWebHostEnvironment hostingEnvironment)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _hostingEnvironment = hostingEnvironment;
        }

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
            return View(model);
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

    }
}
