using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class FaqRepository : IFaqRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Faq> _faqs;

        public FaqRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _faqs = _dbContext.Faqs;
        }

        public IEnumerable<Faq> GetAll()
        {
            return _faqs.OrderBy(c => c.Id).ToList();
        }

        public IEnumerable<Faq> GetBySearchstring(string searchstring)
        {
            return _faqs.Where(f => f.Problem.Contains(searchstring));
        }
    }
}
