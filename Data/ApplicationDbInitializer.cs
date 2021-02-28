using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                //makes company
                Company company = new Company() { CompanyName = "HansAnders", CompanyAdress = "grove Street", CustomerInitDate = DateTime.Now };
                //makes contracts
                //Contract contractRunning1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Running, Type = ContractEnum.type.Weekend };
                //Contract contractRunning2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Running, Type = ContractEnum.type.Weekdays };
                //Contract contractInProgress1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.InProgress, Type = ContractEnum.type.Weekend };
                //Contract contractInProgress2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.InProgress, Type = ContractEnum.type.Weekdays };
                //Contract contractEnded1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Ended, Type = ContractEnum.type.Weekdays };
                //Contract contractEnded2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = ContractEnum.type.Weekend };

                //adds contracts to company
                //Contract[] contracts = new Contract[] { contractRunning1, contractRunning2, contractInProgress1, contractInProgress2, contractEnded1, contractEnded2 };
                //company.Contracts = contracts;
                //makes contactpeople
                _dbContext.Company.Add(company);
                //makes contactpersons and adds to company
                string Username = "NathanT";
                IdentityUser user = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user, "Paswoord_1");
                ContactPerson contactPerson1 = new ContactPerson { user = user, Company = company };
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));
                Username = "StefB";
                IdentityUser user2 = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user2, "Paswoord_1");
                ContactPerson contactPerson2 = new ContactPerson { user = user2, Company = company };
                await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "Customer"));
                //makes tickets
                Ticket ticket1 = new Ticket() { Title = "printer werkt niet.", Status = TicketEnum.status.Created, Type = TicketEnum.type.NoImpact, Description = "ik probeerde iets af te drukken maar het lukte niet kreeg foutmelding x15dc..... " };
                Ticket ticket2 = new Ticket() { Title = "printer staat in BRAND!", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.ProductionWillStop, Description = "printer staat in brand achter dat ik op afdrukken klikte ontstond er een vlam " };
                contactPerson1.Tickets = new Ticket[] { ticket1 };
                contactPerson2.Tickets = new Ticket[] { ticket2 };
                _dbContext.contactPeople.AddRange(contactPerson2, contactPerson1);
                _dbContext.SaveChanges();
            }
        }
    }
}
