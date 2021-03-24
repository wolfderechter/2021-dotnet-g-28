using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using Xunit;

namespace _2021_dotnet_g_28.Tests
{
    public class TicketTest
    {

        private readonly ContactPerson _contactPerson;
        public TicketTest()
        {
            string Username = "ZowieV";
            IdentityUser user = new IdentityUser { UserName = Username };
            Company company = new Company() { CompanyName = "Volvo", CompanyAdress = "grove Street", CustomerInitDate = DateTime.Now };
            _contactPerson = new ContactPerson { User = user, Company = company, FirstName = "Zowie", LastName = "Verschuere" };

        }
        [Fact]
        public void NewTicket_ValidTitle_ValidType_CreateTicket()
        { 
            var dat = DateTime.Now;
            var title = "Malfunction in main line";
            var description = "The factory stopped producing because of a fault in the main line";
            var ticket = new Ticket() { DateCreation = dat, Title = title, Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = description, ContactPersonId = _contactPerson.Id };
            Assert.Equal(dat, ticket.DateCreation);
            Assert.Equal(title, ticket.Title);
            Assert.Equal(TicketEnum.Status.Created, ticket.Status);
            Assert.Equal(TicketEnum.Type.ProductionStopped, ticket.Type);
            Assert.Equal(description, ticket.Description);
             Assert.Equal(_contactPerson.Id, ticket.ContactPersonId);

        }

        [Fact]
        public void EditTicket_ValidTitle_EditsTicket()
        {
            var dat = DateTime.Now;
            var ticket = new Ticket() { DateCreation = dat, Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", ContactPersonId = _contactPerson.Id };
            var title = "This is a new title";
            List<string> files = new List<string>();
            ticket.EditTicket(title, ticket.Description, ticket.Type, files);

            Assert.Equal(title, ticket.Title);
        }

        [Fact]
        public void EditTicket_ValidType_EditsTicket()
        {
            var dat = DateTime.Now;
            var ticket = new Ticket() { DateCreation = dat, Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", ContactPersonId = _contactPerson.Id };
            var type = TicketEnum.Type.NoImpact;
            List<string> files = new List<string>();
            ticket.EditTicket(ticket.Title, ticket.Description, type, files);

            Assert.Equal(type, ticket.Type);
        }

        [Fact]
        public void EditTicket_ValidDescription_EditsTicket()
        {
            var dat = DateTime.Now;
            var ticket = new Ticket() { DateCreation = dat, Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", ContactPersonId = _contactPerson.Id };
            var description = "This is a new Title";
            List<string> files = new List<string>();
            ticket.EditTicket(ticket.Title, description, ticket.Type, files);

            Assert.Equal(description, ticket.Description);
        }

        [Fact]
        public void EditTicket_ValidAttachments_EditsTicket()
        {
            var dat = DateTime.Now;
            var ticket = new Ticket() { DateCreation = dat, Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", ContactPersonId = _contactPerson.Id };
            List<string> files = new List<string>();
            files.Add("image.jpg");
            ticket.EditTicket(ticket.Title, ticket.Description, ticket.Type, files);
            Assert.Equal(files, ticket.Attachments);
        }

        [Fact]
        public void EditTicket_InvalidEdit_ThrowsArgumentException()
        {
            List<string> files = new List<string>();
            files.Add("image.jpg");
            var dat = DateTime.Now;
            var ticket = new Ticket() { DateCreation = dat, Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.ProductionStopped, Description = "The factory stopped producing because of a fault in the main line ", ContactPersonId = _contactPerson.Id };
            Assert.Throws<ArgumentException>(() => ticket.EditTicket( "", null, TicketEnum.Type.NoImpact, files));

        }
    }
}