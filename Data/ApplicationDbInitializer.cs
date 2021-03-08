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
            //_dbContext.Database.EnsureDeleted();
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
                ContactPerson contactPerson1 = new ContactPerson { User = user, Company = company };
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));
                Username = "StefB";
                IdentityUser user2 = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user2, "Paswoord_1");
                ContactPerson contactPerson2 = new ContactPerson { User = user2, Company = company };
                await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "Customer"));
                //makes tickets
                Ticket ticket1 = new Ticket() { Title = "printer werkt niet.", Status = TicketEnum.status.Created, Type = TicketEnum.type.NoImpact, Description = "ik probeerde iets af te drukken maar het lukte niet kreeg foutmelding x15dc..... ", ContactPersonId = contactPerson1.Id };
                Ticket ticket2 = new Ticket() { Title = "computer kapot.", Status = TicketEnum.status.Created, Type = TicketEnum.type.NoImpact, Description = "computer wil niet meer opstarten.", ContactPersonId = contactPerson2.Id };
                Ticket ticket3 = new Ticket() { Title = "printer staat in BRAND!", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.ProductionWillStop, Description = "printer staat in brand achter dat ik op afdrukken klikte ontstond er een vlam ", ContactPersonId = contactPerson2.Id };
                Ticket ticket4 = new Ticket() { Title = "server ontploft.", Status = TicketEnum.status.ResponseReceived, Type = TicketEnum.type.ProductionStopped, Description = "server is ontploft.", ContactPersonId = contactPerson2.Id };
                Ticket ticket5 = new Ticket() { Title = "airco gestopt.", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.NoImpact, Description = "airco is gestopt met werken.", ContactPersonId = contactPerson2.Id };
                Ticket ticket6 = new Ticket() { Title = "computer freeze.", Status = TicketEnum.status.Closed, Type = TicketEnum.type.NoImpact, Description = "computer bevriest heel de tijd.", ContactPersonId = contactPerson2.Id };

                Faq faq1 = new Faq() { Problem = "Netwerk storing", Solution = "Router opnieuw opstarten" };
                Faq faq2 = new Faq() { Problem = "Computer vastegelopen", Solution = "Trek de stekker uit en start opnieuw op"};

                
                contactPerson1.AddTicket(ticket1);
                
                contactPerson2.AddTicket(ticket2);
                contactPerson2.AddTicket(ticket3);
                contactPerson2.AddTicket(ticket4);
                contactPerson2.AddTicket(ticket5);
                contactPerson2.AddTicket(ticket6);
                _dbContext.Faq.AddRange(faq1, faq2);

                _dbContext.contactPeople.AddRange(contactPerson2, contactPerson1);
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
