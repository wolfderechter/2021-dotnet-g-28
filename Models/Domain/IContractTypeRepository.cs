using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public interface IContractTypeRepository
    {
        public IEnumerable<ContractType> GetAllActive();
        ContractType GetByName(string typeName);
    }
}
