using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Company 
    {
        #region properties
        public int CompanyNr { get; set; }
        public String CompanyAdress { get; set; }
        public String CompanyName { get; set; }
        public DateTime CustomerInitDate { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<ContactPerson> ContactPersons { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
        //public ICollection<String> TelNrs { get; set; }
        #endregion

        public Company()
        {
            Contracts = new List<Contract>();
            Tickets = new List<Ticket>();
        }
        #region methods
        public void AddContract(Contract contract)
        {
            
            if (contract.Status == ContractEnum.status.InProgress || contract.Status == ContractEnum.status.Running) 
            {
                if (Contracts.Where(c=>c.Type == contract.Type).Any(c => c.Status == ContractEnum.status.InProgress || c.Status == ContractEnum.status.Running))
                    throw new ArgumentException($"A company can only have one contract with status {contract.Status}.");
            }
            Contracts.Add(contract);
        }

        public void ConsultContract()
        {
            throw new NotImplementedException();
        }

        public void EndContract()
        {
            throw new NotImplementedException();
        }

        public void AddTicket(Ticket ticket)
        {
            Tickets.Add(ticket);
        }

        public void EndTicket()
        {
            throw new NotImplementedException();
        }

        public void EditTicket()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
