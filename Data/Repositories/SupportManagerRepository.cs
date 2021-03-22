using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Repositories
{
    public class SupportManagerRepository : ISupportManagerRepository
    {
        private readonly DbSet<SupportManager> _supportManagers;
        private readonly ApplicationDbContext _dbContext;
        public SupportManagerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _supportManagers = dbContext.SupportManagers;
        }

        public SupportManager GetById(string id)
        {
            return _supportManagers.Single(s=>s.User.Id == id);
        }
    }
}
