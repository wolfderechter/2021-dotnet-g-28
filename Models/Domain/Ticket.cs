using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Ticket
    {
        public int TicketNr { get; set; }
        public String Status { get; set; }
        public DateTime DateCreation { get; set; }
       public String Title { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public String Remark { get; set; }
        public String Attachements { get; set; }


        public Ticket(DateTime dateCreation, string title, string description, string type, string attatchements = "")
        {
            //ticketNr nog onduidelijk
            DateCreation = dateCreation;
            Title = title;
            Description = description;
            Type = type;
            Attachements = attatchements;
        }


    }

    
}
