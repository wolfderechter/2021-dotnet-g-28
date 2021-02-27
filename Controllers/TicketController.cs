using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
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
