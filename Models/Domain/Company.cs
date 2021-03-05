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

        public String CompanyAdress { get; set; }
        public String CompanyName { get; set; }
        public int CompanyNr { get; set; }
        public DateTime CustomerInitDate { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<ContactPerson> ContactPersons { get; set; }
        //public ICollection<String> TelNrs { get; set; }
        #endregion

        #region methods
        public void AddContract(Contract contract)
        {
            if (contract.Status == ContractEnum.status.InProgress || contract.Status == ContractEnum.status.Running) 
            {
                if (Contracts.Any(c => c.Status == contract.Status))
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

        public void CreateTicket()
        {
            throw new NotImplementedException();
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
