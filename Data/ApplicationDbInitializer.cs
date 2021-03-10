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
                Contract contractInProgress1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Cancelled, Type = contractType3 };
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
                
                
                Ticket ticket1 = new Ticket() { DateCreation = DateTime.Now, Title = "printer werkt niet.", Status = TicketEnum.status.Created, Type = TicketEnum.type.NoImpact, Description = "ik probeerde iets af te drukken maar het lukte niet kreeg foutmelding x15dc..... ", ContactPersonId = contactPerson1.Id };
                Ticket ticket2 = new Ticket() { DateCreation = DateTime.Now, Title = "computer kapot.", Status = TicketEnum.status.Created, Type = TicketEnum.type.NoImpact, Description = "computer wil niet meer opstarten.", ContactPersonId = contactPerson2.Id };
                Ticket ticket3 = new Ticket() { DateCreation = DateTime.Now, Title = "printer staat in BRAND!", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.ProductionWillStop, Description = "printer staat in brand achter dat ik op afdrukken klikte ontstond er een vlam ", ContactPersonId = contactPerson2.Id };
                Ticket ticket4 = new Ticket() { DateCreation = DateTime.Now, Title = "server ontploft.", Status = TicketEnum.status.ResponseReceived, Type = TicketEnum.type.ProductionStopped, Description = "server is ontploft.", ContactPersonId = contactPerson2.Id };
                Ticket ticket5 = new Ticket() { DateCreation = DateTime.Now, Title = "airco gestopt.", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.NoImpact, Description = "airco is gestopt met werken.", ContactPersonId = contactPerson2.Id };
                Ticket ticket6 = new Ticket() { DateCreation = DateTime.Now, Title = "computer freeze.", Status = TicketEnum.status.Closed, Type = TicketEnum.type.NoImpact, Description = "computer bevriest heel de tijd.", ContactPersonId = contactPerson2.Id };
                
                Reaction reaction1 = new Reaction() { IsSolution = false, ReactionSup = true, NameUserReaction = "Fred HelpDesk", Text = "Did you try this solution." };
                Reaction reaction2 = new Reaction() { IsSolution = false, ReactionSup = false, NameUserReaction = "Femke Klant", Text = "your solution didn't work." };
                Reaction reaction3 = new Reaction() { IsSolution = true, ReactionSup = true, NameUserReaction = "Fred HelpDesk", Text = "Did you try this other solution." };
                ticket1.Reactions = new List<Reaction>() { reaction1, reaction2, reaction3 };

                Faq faq1 = new Faq() { Problem = "HTTP ERROR 401 (UNAUTHORIZED)", Solution = "1. Check the URL for errors. <br>  2. If the URL is correct,  go to the site’s homepage and look for a login link.Enter your username and password, and then try the page again. <br>  3. If the page you’re trying to access isn’t supposed to need authorization, contact the webmaster and let them know." };
                Faq faq2 = new Faq() { Problem = "HTTP ERROR 400 (BAD REQUEST)", Solution = "1. Check for errors in the URL. This is the most common reason for a 400 Bad Request error. Make sure to check for syntax errors! <br>  2. Clear your browser’s cookies. Sites can sometimes report a 400 error if the cookie it’s reading is corrupt. <br>  3. Clear your DNS cache. If you don’t know how to do this, read these instructions! <br>  4. Clear your browser’s cache, here’s how!" };
                Faq faq3 = new Faq() { Problem = "HTTP ERROR 403 (FORBIDDEN)", Solution = "1. Check for an error in the URL. <br> 2. Clear your browser’s cache and cookies.If you don’t know how to do this, read these instructions! <br> 3. Contact your service provider if the issue is still not resolved." };
                Faq faq4 = new Faq() { Problem = "HTTP ERROR 404 (NOT FOUND)", Solution = "1. Refresh page. <br> 2. Check the URL for errors. <br> 3. Clear your browser’s cache and cookies. If you don’t know how to do this, read these instructions! <br> 4. Scan your computer for malware, here’s how. <br> 5. Contact the Webmaster and let them know about the issue." };
                Faq faq5 = new Faq() { Problem = "SMART Hard Disk Error 301", Solution = "this error indicates that the hard disk or solid-state drive has already experienced a failure, or will soon. This error message will appear when you turn on the device and can cause serious damage if not treated immediately. It could be the result of a broken controller chip, failed installation of an application, a power surge,  or malware. Sometimes, a user can change the BIOS sequence or attempt a reboot, but if the drive has a physical error, then it is best not to run the computer to avoid further damage." };

                contactPerson1.AddTicket(ticket1);
                
                contactPerson2.AddTicket(ticket2);
                contactPerson2.AddTicket(ticket3);
                contactPerson2.AddTicket(ticket4);
                contactPerson2.AddTicket(ticket5);
                contactPerson2.AddTicket(ticket6);
                _dbContext.Faq.AddRange(faq1, faq2, faq3, faq4, faq5);

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
