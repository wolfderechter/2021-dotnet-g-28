using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public interface IContractRepository
    {
        IEnumerable<Contract> GetByIdAndStatus(IEnumerable<ContractEnum.status> statussen,int companyNr);
        IEnumerable<Contract> GetAll();
        Contract GetById(int id);
        void Add(Contract contract);
        void ChangeStatus(int contractNr,ContractEnum.status status);
        void SaveChanges();

        void Delete(Contract contract);
    }
}
