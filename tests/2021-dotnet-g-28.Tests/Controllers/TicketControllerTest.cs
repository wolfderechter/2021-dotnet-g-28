using _2021_dotnet_g_28.Controllers;
using _2021_dotnet_g_28.Data;
using _2021_dotnet_g_28.Data.Repositories;
using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace _2021_dotnet_g_28.Tests.Controllers
{
    public class TicketControllerTest
    {
        private readonly TicketController _ticketController;
        private Mock<ITicketRepository> _mockTicketRepository;
        private Mock<IContactPersonRepository> _mockContactRepository;

        public TicketControllerTest()
        {
         
            _mockTicketRepository = new Mock<ITicketRepository>();
            _mockContactRepository = new Mock<IContactPersonRepository>();
            _ticketController = new TicketController(_mockTicketRepository.Object, null, _mockContactRepository.Object, null, null, null)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }




        [Fact]
        public void Index_AllTickets_PassesAllTicketsFilteredByStatusAndType()
        {

            Ticket ticket1 = new Ticket() { DateCreation = DateTime.Now.AddDays(-2), Title = "Malfunction in main line ", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.NoImpact, Description = "The factory stopped producing because of a fault in the main line ", Attachments = new List<string>() };
          //  Ticket ticket2 = new Ticket() { DateCreation = DateTime.Now.AddDays(-1), Title = "Water damage ", Status = TicketEnum.Status.Cancelled, Type = TicketEnum.Type.NoImpact, Description = "We had an water leak and everything is soaked", Attachments = new List<string>() };
          //  Ticket ticket3 = new Ticket() { DateCreation = DateTime.Now.AddDays(0), Title = "Ip error shown on tablet", Status = TicketEnum.Status.InProgress, Type = TicketEnum.Type.ProductionWillStop, Description = "The scanners show an Ip error when trying to scan merchandise ", Attachments = new List<string>() };
            List<Ticket> tickets = new List<Ticket>();
            tickets.Add(ticket1);
            List<TicketEnum.Status> statuses = new List<TicketEnum.Status>();
            statuses.Add(TicketEnum.Status.Created);
            List<TicketEnum.Type> types = new List<TicketEnum.Type>();
            types.Add(TicketEnum.Type.NoImpact);
            _mockTicketRepository.Setup(t => t.GetByStatusAndType(statuses, types)).Returns(tickets);
            

        }

        #region -- Delete GET --
        [Fact]
        public void DeleteHttpGet_RedirectsToIndex()
        {
            var ticket1 = new Ticket() { DateCreation = DateTime.Now, Title = "Malfunction in main line", Status = TicketEnum.Status.Created, Type = TicketEnum.Type.NoImpact, Description = "The factory stopped producing because of a fault in the main line ", Attachments = new List<string>() };
            _mockTicketRepository.Setup(t => t.GetBy(ticket1.TicketNr)).Returns(ticket1);
            _mockTicketRepository.Setup(t => t.Delete(It.IsAny<Ticket>()));
            var result = Assert.IsType<ViewResult>(_ticketController.Delete(ticket1.TicketNr));
            Ticket ticket = (Ticket)result.Model;
            Assert.Equal("Malfunction in main line", ticket.Title);
        }
        #endregion
    }
}

