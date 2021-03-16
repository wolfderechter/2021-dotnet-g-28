﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public interface ITicketRepository
    {
        Ticket GetBy(int ticketNr);
        IEnumerable<Ticket> GetByStatusAndType(IEnumerable<TicketEnum.status> statusses, IEnumerable<TicketEnum.type> types);
        IEnumerable<Ticket> GetByContactPersonId(int contactPersonId);
        IEnumerable<Ticket> GetAll();
        void Add(Ticket ticket);
        void SaveChanges();
        void Delete(Ticket ticket);
    }
}
