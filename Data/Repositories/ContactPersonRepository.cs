using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class ContactPersonRepository : IContactPersonRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ContactPerson> _contactPeople;
        public ContactPersonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            _contactPeople = _dbContext.ContactPeople;
        }
        
        public IEnumerable<ContactPerson> getAll()
        {
            return _contactPeople.Include(c => c.Company).OrderBy(c=>c.Id).ToList();

        }

        public ContactPerson getById(String userId)
        {

            return _contactPeople.Include(c => c.Company).ThenInclude(c=>c.Contracts).Include(c=>c.Notifications).SingleOrDefault(c => c.User.Id == userId);

        }
    }
}
