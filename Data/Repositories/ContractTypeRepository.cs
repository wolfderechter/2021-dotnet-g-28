using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class ContractTypeRepository : IContractTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ContractType> _contractTypes;

        public ContractTypeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _contractTypes = dbContext.ContractTypes;
        }

        public IEnumerable<ContractType> GetAllActive()
        {
            return _contractTypes.Where(c=>c.IsActive).ToList();
        }

        public ContractType GetByName(string typeName)
        {
            return _contractTypes.SingleOrDefault(c => c.Name == typeName);
        }
    }
}
