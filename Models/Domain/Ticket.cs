using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Ticket
    {
        public int TicketNr { get; set; }
        public TicketEnum.status Status { get; set; }
        public DateTime DateCreation { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public TicketEnum.type Type { get; set; }
        public String Remark { get; set; }
        public String Attachments { get; set; }


        public Ticket(DateTime dateCreation, string title, string description, TicketEnum.type type, string attatchements = "")
        {
            //ticketNr nog onduidelijk
            DateCreation = dateCreation;
            Title = title;
            Description = description;
            Type = type;
            Attachments = attatchements;
        }
        public Ticket(string title, string description, TicketEnum.type type)
        {
            Title = title;
            Description = description;
            Type = type;
        }
        public Ticket()
        {

        }


    }

    
}
