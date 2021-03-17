using Microsoft.AspNetCore.Http;
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
        public int CompanyNr { get; set; }
        public string PicturePath { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public Ticket()
        {
            Reactions = new List<Reaction>();
        }

        public Ticket(DateTime dateCreation, string title, string remark, string description, TicketEnum.type type, TicketEnum.status status, string filePath)
        {
            DateCreation = dateCreation;
            Title = title;
            Remark = remark;
            Description = description;
            Type = type;
            Status = status;
            PicturePath = filePath;
        }
        public Ticket(DateTime dateCreation, string title, string remark, string description, TicketEnum.type type, TicketEnum.status status)
        {
            DateCreation = dateCreation;
            Title = title;
            Remark = remark;
            Description = description;
            Type = type;
            Status = status;
        }


       

        public void EditTicket(string title, string remark, string description, TicketEnum.type type, string filePath)
        {
            Title = title;
            Remark = remark;
            Description = description;
            Type = type;
            PicturePath = filePath;
        }

        public void AddReaction(Reaction reaction)
        {
            Reactions.Add(reaction);

        }
    }

    
}
