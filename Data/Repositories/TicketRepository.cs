using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Ticket> _tickets;

        public TicketRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _tickets = dbContext.Tickets;
        }

        public Ticket GetBy(int ticketNr)
        {
            return _tickets.FirstOrDefault(t => t.TicketNr == ticketNr);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _tickets.ToList();
        }

        public void Add(Ticket ticket)
        {
            _tickets.Add(ticket);
        }

        public void Delete(Ticket ticket)
        {
            ticket.Status = TicketEnum.status.Cancelled;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Ticket> GetByContactPersonId(int companyNr)
        {
            return _tickets.Include(t=>t.Reactions).Where(t => t.CompanyNr == companyNr).ToList();
            //return _tickets.Include(t => t.Reactions).Where(t => t.ContactPersonId == contactPersonId).ToList();
        }

        public IEnumerable<Ticket> GetByStatusAndType(IEnumerable<TicketEnum.status> statusses, IEnumerable<TicketEnum.type> types)
        {
            return _tickets.Include(t => t.Reactions).Where(c => statusses.Contains(c.Status) && types.Contains(c.Type)).ToList();
        }
    }
}
