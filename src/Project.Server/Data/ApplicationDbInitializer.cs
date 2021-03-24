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
                Company HansAnders = new Company() { CompanyName = "HansAnders", CompanyAdress = "grove Street", CustomerInitDate =DateTime.Now};
                Company DovyKeukens = new Company() { CompanyName = "DovyKeukens", CompanyAdress = "Ballstreet", CustomerInitDate = DateTime.Now };
                //makes ContractType
                ContractType contractType = new ContractType() { Name = "BusinessHours E-Mail" ,IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email,MaxResponseTime=24,IsOutsideBusinessHours=false,Price=150, MinDuration = 1 };
                ContractType contractType2 = new ContractType() { Name = "Weekend E-Mail", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email, MaxResponseTime = 48,IsOutsideBusinessHours=true, Price = 2000, MinDuration = 2 };
                ContractType contractType3 = new ContractType() { Name = "BusinessHours Phone", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.phone, MaxResponseTime = 48, IsOutsideBusinessHours = false, Price = 1900, MinDuration = 2};
                ContractType contractType4 = new ContractType() { Name = "Weekend Phone", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.phone,MaxResponseTime = 24, IsOutsideBusinessHours = true, Price = 2400, MinDuration = 1};
                ContractType contractType5 = new ContractType() { Name = "BusinessHours App", IsActive = true,CreationMethod=ContractTypeEnum.CreationMethod.app, MaxResponseTime = 12, IsOutsideBusinessHours = false, Price = 2800, MinDuration = 1};
                ContractType contractType6 = new ContractType() { Name = "Weekend App", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.app, MaxResponseTime = 6, IsOutsideBusinessHours = true, Price = 3000,MinDuration=2 };
                //makes contracts
                Contract contractRunning1 = new Contract() {  StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Running, Type= contractType};
                Contract contractRunning2 = new Contract() { StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType2 };
                Contract contractInProgress1 = new Contract() {  StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Cancelled, Type = contractType3 };
                Contract contractInProgress2 = new Contract() {  StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Cancelled, Type = contractType4 };
                Contract contractEnded1 = new Contract() {  StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Ended, Type = contractType5 };
                Contract contractEnded2 = new Contract() {  StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType6 };
                Contract contractDovy = new Contract() { StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType6 };
                //adds contracts to company
                List<Contract> contracts = new List<Contract> { contractRunning1, contractRunning2, contractInProgress1, contractInProgress2, contractEnded1, contractEnded2 };
                HansAnders.Contracts = contracts;
                
                DovyKeukens.Contracts.Add(contractDovy);

                //makes contactpeople
                _dbContext.Companies.AddRange(HansAnders, DovyKeukens);

                //makes contactpersons and adds to company
                string Username = "NathanT";
                IdentityUser user = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user, "Paswoord_1");
                ContactPerson contactPerson1 = new ContactPerson { User = user, Company = HansAnders, FirstName="Nathan",LastName="Tersago"};
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Customer"));

                Username = "StefB";
                IdentityUser user2 = new IdentityUser { UserName = Username };
                await _userManager.CreateAsync(user2, "Paswoord_1");
                SupportManager supportManager = new SupportManager { User = user2, FirstName = "Stef", LastName = "Boerjan"};
                await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "SupportManager"));

                //Username = "ZowieV";
                //IdentityUser user3 = new IdentityUser { UserName = Username };
                //await _userManager.CreateAsync(user, "Paswoord_1");
                //ContactPerson contactPerson2 = new ContactPerson { User = user, Company = HansAnders, FirstName = "Zowie", LastName = "Verschuere" };
                //await _userManager.AddClaimAsync(user3, new Claim(ClaimTypes.Role, "Customer"));
                //makes tickets

                Ticket ticket1 = new Ticket() { DateCreation = DateTime.Now.AddDays(-2), Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", Attachments = new List<string>()};
                Ticket ticket2 = new Ticket() { DateCreation = DateTime.Now.AddDays(-1), Title = "Water damage ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionWillStop, Description = "We had an water leak and everything is soaked", Attachments = new List<string>() };
                Ticket ticket3 = new Ticket() { DateCreation = DateTime.Now.AddDays(0), Title = "Ip error shown on tablet", Status = TicketEnum.Status.InProgress, Type = TicketEnum.Type.ProductionWillStop, Description = "The scanners show an Ip error when trying to scan merchandise ", Attachments = new List<string>()};
                Ticket ticket4 = new Ticket() { DateCreation = DateTime.Now.AddDays(+1), Title = "Server shutdown", Status = TicketEnum.Status.ResponseReceived, Type = TicketEnum.Type.ProductionStopped, Description = "Unable to connect to server message shown every time we try and access the company network", Attachments = new List<string>()};
                Ticket ticket6 = new Ticket() { DateCreation = DateTime.Now.AddDays(+2), Title = "Computer freeze.", Status = TicketEnum.Status.Closed, Type = TicketEnum.Type.NoImpact, Description = "Every time I try to play flappy bird on the company computer the pc freezes and stops working for a few seconds", Attachments = new List<string>() };
                Ticket ticket7 = new Ticket() { DateCreation = DateTime.Now.AddDays(0), Title = "Computer freeze.", Status = TicketEnum.Status.Closed, Type = TicketEnum.Type.NoImpact, Description = "Every time I try to play flappy bird on the company computer the pc freezes and stops working for a few seconds", Attachments = new List<string>() };


                Reaction reaction1 = new Reaction() { IsSolution = false, ReactionSup = true, NameUserReaction = "Rajish Abdul", Text = "is there any sign of malfunction , like a specific area." };
                Reaction reaction2 = new Reaction() { IsSolution = false, ReactionSup = false, NameUserReaction = "Nathan Tersago", Text = "it get's stuck around the compression machine" };
                Reaction reaction3 = new Reaction() { IsSolution = true, ReactionSup = true, NameUserReaction = "Rajish Abdul", Text = "We will send the technician over." };
                ticket1.Reactions = new List<Reaction>() { reaction1, reaction2, reaction3 };

                List<string> solution1 = new List<string>() { "1. Check the URL for errors.", "2. If the URL is correct,  go to the site’s homepage and look for a login link.Enter your username and password, and then try the page again.", "3. If the page you’re trying to access isn’t supposed to need authorization, contact the webmaster and let them know." };
                List<string> solution2 = new List<string>() { "1. Check for errors in the URL. This is the most common reason for a 400 Bad Request error. Make sure to check for syntax errors!", "2. Clear your browser’s cookies. Sites can sometimes report a 400 error if the cookie it’s reading is corrupt.", "3. Clear your DNS cache. If you don’t know how to do this, read these instructions!" ,"4. Clear your browser’s cache, here’s how!" };
                List<string> solution3 = new List<string>() { "1. Check for an error in the URL.", "2. Clear your browser’s cache and cookies.If you don’t know how to do this, read these instructions!", "3. Contact your service provider if the issue is still not resolved." };
                List<string> solution4 = new List<string>() { "1. Refresh page.", "2. Check the URL for errors.", "3. Clear your browser’s cache and cookies. If you don’t know how to do this, read these instructions!", "4. Scan your computer for malware, here’s how.", "5. Contact the Webmaster and let them know about the issue." };

                //make notification
                Notification notification = new Notification() { Action = "Reaction", TicketName = "Water Damage" };
                Notification notification2 = new Notification() { Action = "Reaction", TicketName = "Water Damage" };
                Notification notification3 = new Notification() { Action = "Reaction", TicketName = "Water Damage" };
                Notification notification4 = new Notification() { Action = "Reaction", TicketName = "Water Damage" };

                contactPerson1.Notifications = new List<Notification>() { notification, notification2, notification3, notification4};


                Faq faq1 = new Faq() { Problem = "HTTP ERROR 401 (UNAUTHORIZED)", Solution =  solution1};
                Faq faq2 = new Faq() { Problem = "HTTP ERROR 400 (BAD REQUEST)", Solution =  solution2};
                Faq faq3 = new Faq() { Problem = "HTTP ERROR 403 (FORBIDDEN)", Solution = solution3};
                Faq faq4 = new Faq() { Problem = "HTTP ERROR 404 (NOT FOUND)", Solution =  solution4};
                _dbContext.Faqs.AddRange(faq1, faq2, faq3, faq4);


                HansAnders.AddTicket(ticket1);
                HansAnders.AddTicket(ticket2);
                HansAnders.AddTicket(ticket3);
                DovyKeukens.AddTicket(ticket4);
                DovyKeukens.AddTicket(ticket6);
                DovyKeukens.AddTicket(ticket7);

                _dbContext.ContactPeople.Add(contactPerson1);
                _dbContext.SupportManagers.Add(supportManager);

                _dbContext.SaveChanges();
            }
        }
    }
}
