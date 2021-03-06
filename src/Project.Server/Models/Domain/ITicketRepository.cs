using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public interface ITicketRepository
    {
        Ticket GetBy(int ticketNr);
        IEnumerable<Ticket> GetByStatusAndType(IEnumerable<TicketEnum.Status> statusses, IEnumerable<TicketEnum.Type> types,int companyNr);
        IEnumerable<Ticket> GetByContactPersonId(int companyNr);
        IEnumerable<Ticket> GetAll();
        void Add(Ticket ticket);
        void SaveChanges();
        void Delete(Ticket ticket);
    }
}
