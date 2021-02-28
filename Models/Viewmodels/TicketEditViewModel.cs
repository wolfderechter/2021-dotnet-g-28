using _2021_dotnet_g_28.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class TicketEditViewModel
    {
        //attributes of ticket
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketEnum.type Type { get; set; }


        public TicketEditViewModel()
        {

        }

        public TicketEditViewModel(Ticket ticket)
        {
            //attributes of ticket from ticket
            Title = ticket.Title;
            Description = ticket.Description;
            Type = ticket.Type;
        }
    }
}
