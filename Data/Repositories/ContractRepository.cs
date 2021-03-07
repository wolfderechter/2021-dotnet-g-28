using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Contract> _contracts;
       public ContractRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _contracts = dbContext.Contracts;

        }
        public void Add(Contract contract)
        {
            _contracts.Add(contract);
        }

        public void ChangeStatus(int contractNr, ContractEnum.status status)
        {
            Contract ChangeContract = _contracts.SingleOrDefault(c => c.ContractNr == contractNr);
            ChangeContract.Status = status;
        }

        public void Delete(Contract contract)
        {
            contract.Status = ContractEnum.status.Cancelled;
        }

        public IEnumerable<Contract> GetAll()
        {
            return _contracts;
        }

        public IEnumerable<Contract> GetByIdAndStatusAndDuration(IEnumerable<ContractEnum.status> statussen, IEnumerable<int> Durations, int companyNr)
        {
            return _contracts.Include(c=>c.Type).Where(c => c.Company.CompanyNr == companyNr && statussen.Contains(c.Status) && Durations.Contains(c.EndDate.Year-c.StartDate.Year)).ToList();
        }

        public Contract GetById(int contractNr)
        {
            return _contracts.Include(c=>c.Type).FirstOrDefault(c => c.ContractNr == contractNr);

        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
