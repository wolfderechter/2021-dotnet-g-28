using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public interface ICompanyRepository
    {
        public IEnumerable<Company> GetAll();
        Company GetByNr(int companyNr);
    }
}
