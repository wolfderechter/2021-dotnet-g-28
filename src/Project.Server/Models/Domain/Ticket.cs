﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Ticket
    {
        public int TicketNr { get; set; }
        public TicketEnum.Status Status { get; set; }
        public DateTime DateCreation { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public TicketEnum.Type Type { get; set; }
        public int CompanyNr { get; set; }
        public int ContactPersonId { get; set; }
        public string PicturePath { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public List<string> Attachments { get; set; }
        public DateTime Now { get; }
        public TicketEnum.Type Type1 { get; }
        public TicketEnum.Status Created { get; }

        public Ticket()
        {
            Reactions = new List<Reaction>();
        }

        public Ticket(DateTime dateCreation, string title, string description, TicketEnum.Type type, TicketEnum.Status status)
        {
          
            DateCreation = dateCreation;
            Title = title;
            Description = description;
            Type = type;
            Status = status;
            Attachments = new List<string>();
        }
  


        public void EditTicket(string title, string description, TicketEnum.Type type)
        {
            Title = title;
            Description = description;
            Type = type;

        }

        public void AddReaction(Reaction reaction)
        {
            Reactions.Add(reaction);

        }
    }

    
}