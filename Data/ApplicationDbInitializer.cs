using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data
{
    public class ApplicationDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationDbInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public  async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                await InitializeUsers();
                _dbContext.Company.Add(new Company() { CompanyName = "HansAnders" });
                _dbContext.SaveChanges();
            }

            
        }

        private async Task InitializeUsers()
        {
            string Username = "NathanT";
            IdentityUser user = new IdentityUser { UserName = Username };
            await _userManager.CreateAsync(user, "Paswoord_1");
           // await _userManager.AddToRoleAsync(
            Username = "StefB";
            user = new IdentityUser { UserName = Username, AccessFailedCount=3};
            await _userManager.CreateAsync(user, "Paswoord_1");
        }
    }
}
