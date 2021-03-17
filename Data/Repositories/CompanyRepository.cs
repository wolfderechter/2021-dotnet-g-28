using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Company> _companies;

        public CompanyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _companies = dbContext.Companies;
            
        }

        public IEnumerable<Company> GetAll()
        {
           return _companies;
        }

        public Company GetByNr(int companyNr)
        {
            return _companies.Include(c => c.Tickets).First(c=>c.CompanyNr == companyNr);
        }
    }
}
