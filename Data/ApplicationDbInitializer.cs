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

        public  async Task InitializeData()
        {
            //_dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            { 
                //makes company
                Company company = new Company() { CompanyName = "HansAnders", CompanyAdress = "grove Street", CustomerInitDate =DateTime.Now};
                //makes ContractType
                ContractType contractType = new ContractType() { Name = "StandaardContract" ,IsActive = true};
                ContractType contractType2 = new ContractType() { Name = "contract1", IsActive = true };
                ContractType contractType3 = new ContractType() { Name = "contract2", IsActive = true };
                ContractType contractType4 = new ContractType() { Name = "contract3", IsActive = true };
                ContractType contractType5 = new ContractType() { Name = "contract4", IsActive = true };
                //makes contracts
                Contract contractRunning1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Running, Type= contractType };
                Contract contractRunning2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType2 };
                Contract contractInProgress1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.NotActive, Type = contractType3 };
                Contract contractInProgress2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Cancelled, Type = contractType4 };
                Contract contractEnded1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Ended, Type = contractType5 };
                Contract contractEnded2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType };

                //adds contracts to company
                List<Contract> contracts = new List<Contract> { contractRunning1, contractRunning2, contractInProgress1, contractInProgress2, contractEnded1, contractEnded2 };
                company.Contracts = contracts;
                //makes contactpeople
                _dbContext.Company.Add(company);
                //makes contactpersons and adds to company
                string Username = "NathanT";
                IdentityUser user = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user, "Paswoord_1");
                ContactPerson contactPerson1 = new ContactPerson { User = user, Company = company };
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));
                Username = "StefB";
                IdentityUser user2 = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user2, "Paswoord_1");
                ContactPerson contactPerson2 = new ContactPerson { User = user2, Company = company };
                await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "Customer"));
                //makes tickets
                Ticket ticket1 = new Ticket() { Title = "printer werkt niet.",Status= TicketEnum.status.Created,Type=TicketEnum.type.NoImpact,Description="ik probeerde iets af te drukken maar het lukte niet kreeg foutmelding x15dc..... "};
                Ticket ticket2 = new Ticket() { Title = "printer staat in BRAND!", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.ProductionWillStop, Description = "printer staat in brand achter dat ik op afdrukken klikte ontstond er een vlam " };
                contactPerson1.Tickets = new Ticket[] { ticket1 };
                contactPerson2.Tickets = new Ticket[] { ticket2 };
                _dbContext.ContactPeople.AddRange(contactPerson2, contactPerson1);
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
