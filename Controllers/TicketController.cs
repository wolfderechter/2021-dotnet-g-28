using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
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
        public TicketController(ITicketRepository ticketRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewData["IsEdit"] = false;
            ViewData["typeTickets"] = TypeTickets();
            return View();
            //return View(nameof(Edit), new TicektEditViewModel());
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
                    //nog aanvullen
                    var ticket = new Ticket(ticketEditViewModel.Title, ticketEditViewModel.Description, ticketEditViewModel.Type);
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
            else {
                //return View(nameof(Edit), ticketEditViewModel);
                return View(nameof(Index), ticketEditViewModel);

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
            try
            {
                Ticket ticket = _ticketRepository.GetBy(ticketNr);
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
    }
}
