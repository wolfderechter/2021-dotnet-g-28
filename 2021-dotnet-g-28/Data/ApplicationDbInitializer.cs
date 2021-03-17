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
                Company company = new Company() { CompanyName = "HansAnders", CompanyAdress = "grove Street", CustomerInitDate =DateTime.Now};
                //makes ContractType
                ContractType contractType = new ContractType() { Name = "BusinessHours E-Mail" ,IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email,MaxResponseTime=24,IsOutsideBusinessHours=false,Price=150, MinDuration = 1 };
                ContractType contractType2 = new ContractType() { Name = "Weekend E-Mail", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email, MaxResponseTime = 48,IsOutsideBusinessHours=true, Price = 2000, MinDuration = 2 };
                ContractType contractType3 = new ContractType() { Name = "BusinessHours Phone", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.phone, MaxResponseTime = 48, IsOutsideBusinessHours = false, Price = 1900, MinDuration = 2};
                ContractType contractType4 = new ContractType() { Name = "Weekend Phone", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.phone,MaxResponseTime = 24, IsOutsideBusinessHours = true, Price = 2400, MinDuration = 1};
                ContractType contractType5 = new ContractType() { Name = "BusinessHours App", IsActive = true,CreationMethod=ContractTypeEnum.CreationMethod.app, MaxResponseTime = 12, IsOutsideBusinessHours = false, Price = 2800, MinDuration = 1};
                ContractType contractType6 = new ContractType() { Name = "Weekend App", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.app, MaxResponseTime = 6, IsOutsideBusinessHours = true, Price = 3000,MinDuration=2 };
                //makes contracts
                Contract contractRunning1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Running, Type= contractType};
                Contract contractRunning2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType2 };
                Contract contractInProgress1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Cancelled, Type = contractType3 };
                Contract contractInProgress2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Cancelled, Type = contractType4 };
                Contract contractEnded1 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Ended, Type = contractType5 };
                Contract contractEnded2 = new Contract() { Company = company, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType6 };

                //adds contracts to company
                List<Contract> contracts = new List<Contract> { contractRunning1, contractRunning2, contractInProgress1, contractInProgress2, contractEnded1, contractEnded2 };
                company.Contracts = contracts;

                //makes contactpeople
                _dbContext.Company.Add(company);
                //makes contactpersons and adds to company
                string Username = "NathanT";
                IdentityUser user = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user, "Paswoord_1");
                ContactPerson contactPerson1 = new ContactPerson { User = user, Company = company,FirstName="Nathan",LastName="Tersago"};
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));
                Username = "StefB";
                IdentityUser user2 = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user2, "Paswoord_1");
                ContactPerson contactPerson2 = new ContactPerson { User = user2, Company = company, FirstName = "Stef", LastName = "Boerjan" };
                await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "Customer"));
                //makes tickets
                
                
                Ticket ticket1 = new Ticket() { DateCreation = DateTime.Now, Title = "Malfunction in main line ", Status = TicketEnum.status.Created, Type = TicketEnum.type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", ContactPersonId = contactPerson1.Id /*,Remark="Very Urgent"*/};
                Ticket ticket2 = new Ticket() { DateCreation = DateTime.Now, Title = "Water damage ", Status = TicketEnum.status.Created, Type = TicketEnum.type.ProductionWillStop, Description = "We had an water leak and everything is soaked", ContactPersonId = contactPerson2.Id };
                Ticket ticket3 = new Ticket() { DateCreation = DateTime.Now, Title = "Ip error shown on tablet", Status = TicketEnum.status.InProgress, Type = TicketEnum.type.ProductionWillStop, Description = "The scanners show an Ip error when trying to scan merchandise ", ContactPersonId = contactPerson2.Id };
                Ticket ticket4 = new Ticket() { DateCreation = DateTime.Now, Title = "Server shutdown", Status = TicketEnum.status.ResponseReceived, Type = TicketEnum.type.ProductionStopped, Description = "Unable to connect to server message shown every time we try and access the company network", ContactPersonId = contactPerson2.Id };
                Ticket ticket6 = new Ticket() { DateCreation = DateTime.Now, Title = "Computer freeze.", Status = TicketEnum.status.Closed, Type = TicketEnum.type.NoImpact, Description = "Every time I try to play flappy bird on the company computer the pc freezes and stops working for a few seconds", ContactPersonId = contactPerson2.Id };
                
                Reaction reaction1 = new Reaction() { IsSolution = false, ReactionSup = true, NameUserReaction = "Rajish Abdul", Text = "is there any sign of malfunction , like a specific area." };
                Reaction reaction2 = new Reaction() { IsSolution = false, ReactionSup = false, NameUserReaction = "Nathan Tersago", Text = "it get's stuck around the compression machine" };
                Reaction reaction3 = new Reaction() { IsSolution = true, ReactionSup = true, NameUserReaction = "Rajish Abdul", Text = "We will send the technician over." };
                ticket1.Reactions = new List<Reaction>() { reaction1, reaction2, reaction3 };

                List<string> solution1 = new List<string>() { "1. Check the URL for errors.", "2. If the URL is correct,  go to the site’s homepage and look for a login link.Enter your username and password, and then try the page again.", "3. If the page you’re trying to access isn’t supposed to need authorization, contact the webmaster and let them know." };
                List<string> solution2 = new List<string>() { "1. Check for errors in the URL. This is the most common reason for a 400 Bad Request error. Make sure to check for syntax errors!", "2. Clear your browser’s cookies. Sites can sometimes report a 400 error if the cookie it’s reading is corrupt.", "3. Clear your DNS cache. If you don’t know how to do this, read these instructions! <br>  4. Clear your browser’s cache, here’s how!" };
                List<string> solution3 = new List<string>() { "1. Check for an error in the URL.", "2. Clear your browser’s cache and cookies.If you don’t know how to do this, read these instructions!", "3. Contact your service provider if the issue is still not resolved." };
                List<string> solution4 = new List<string>() { "1. Refresh page.", "2. Check the URL for errors.", "3. Clear your browser’s cache and cookies. If you don’t know how to do this, read these instructions!", "4. Scan your computer for malware, here’s how.", "5. Contact the Webmaster and let them know about the issue." };



                Faq faq1 = new Faq() { Problem = "HTTP ERROR 401 (UNAUTHORIZED)", Solution =  solution1};
                Faq faq2 = new Faq() { Problem = "HTTP ERROR 400 (BAD REQUEST)", Solution =  solution2};
                Faq faq3 = new Faq() { Problem = "HTTP ERROR 403 (FORBIDDEN)", Solution = solution3};
                Faq faq4 = new Faq() { Problem = "HTTP ERROR 404 (NOT FOUND)", Solution =  solution4};
               
          


                contactPerson1.AddTicket(ticket1);
                
                contactPerson2.AddTicket(ticket2);
                contactPerson2.AddTicket(ticket3);
                contactPerson2.AddTicket(ticket4);
                contactPerson2.AddTicket(ticket6);
                _dbContext.Faq.AddRange(faq1, faq2, faq3, faq4);

                _dbContext.ContactPeople.AddRange(contactPerson2, contactPerson1);

                _dbContext.SaveChanges();
            }
        }
    }
}
