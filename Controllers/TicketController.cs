using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2021_dotnet_g_28.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
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
        public IActionResult Create(TicketEditViewModel ticketEditViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ticket = new Ticket();
                    _ticketRepository.Add(ticket);
                    //_ticketRepository.SaveChanges();
                    TempData["message"] = $"You successfully created a ticket.";
                    Console.WriteLine();
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
            var typeTickets = new List<TypeTicket>();
            foreach (TypeTicket typeTicket in Enum.GetValues(typeof(TypeTicket)))
            {
                typeTickets.Add(typeTicket);
            }
            return new SelectList(typeTickets);
        }
    }
}
