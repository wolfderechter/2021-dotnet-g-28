using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class TicketIndexViewModel
    {
        
        public IEnumerable<Ticket> Tickets;
        public List<StatusModelTicket> CheckBoxItemsStatus { get; set; }
        public List<TypeModelTicket> CheckBoxItemsType { get; set; }
        public Ticket OpenedTicket { get; set; }

    }
}
