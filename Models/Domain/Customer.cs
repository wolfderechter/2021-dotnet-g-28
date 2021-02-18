using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Customer : User
    {
        #region properties
        public IList<ContactPerson> ContactPersons { get; set; }
        public IList<Contract> Contracts { get; set; }

        public String CompanyAdress { get; set; }
        public String CompanyName { get; set; }
        public int CustomerNr { get; set; }
        public DateTime CustomerInitDate { get; set; }
        public IList<String> TelNrs { get; set; }
        #endregion

        #region methods
        public void CreateContract()
        {
            throw new NotImplementedException();
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
