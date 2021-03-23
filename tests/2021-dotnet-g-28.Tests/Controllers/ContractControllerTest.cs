using _2021_dotnet_g_28.Controllers;
using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace _2021_dotnet_g_28.Tests.Controllers
{
    public class ContractControllerTest
    {
        private ContractController _controller;
        private Mock<IContractRepository> _contractRepository;
        private Mock<IContactPersonRepository> _contactPersonRepository;
        //private Mock<UserManager<IdentityUser>> _userManager;

        public ContractControllerTest()
        {
            _contractRepository = new Mock<IContractRepository>();
            _contactPersonRepository = new Mock<IContactPersonRepository>();
            //_userManager = new Mock<UserManager<IdentityUser>>();
            _controller = new ContractController(_contractRepository.Object, null, _contactPersonRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };

        }
        [Fact(Skip ="Navragen leerkracht meer informatie")]
        public void Index_PassedOrderedListOfContractsFromUsersCompanyInViewResultModel()
        {
           // je moet identity framework niet testen
           // arrange
            //string userId = "1234";
            //IdentityUser user = new IdentityUser() { Id = userId };
           //_userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(await user);
            int companyNr = 12;
            ContactPerson person = new ContactPerson() { User = user, Company = new Company() { CompanyNr = companyNr } };
            _contactPersonRepository.Setup(c => c.getById(It.IsAny<String>())).Returns(person);
            Contract contract1 = new Contract() { ContractNr = 1, Company = new Company() { CompanyNr = companyNr } };
            Contract contract2 = new Contract() { ContractNr = 2, Company = new Company() { CompanyNr = companyNr } };
            List<Contract> contracts = new List<Contract>() { contract1, contract2 };
            _contractRepository.Setup(c => c.GetByIdAndStatus(new List<ContractEnum.status>() { ContractEnum.status.InProgress, ContractEnum.status.Running }, 12)).Returns(contracts);
            
            //act
            var result = Assert.IsType<ViewResult>(_controller.Index());

            //assert
        }
        #region == Delete ==
        [Fact]
        public void Delete_ProductFound_DeletesContractAndRedirectsToIndex()
        {
            //arrange
            Contract contract = new Contract() { ContractNr = 1, Status = ContractEnum.status.InProgress };
            _contractRepository.Setup(c => c.Delete(It.IsAny<Contract>()));
            _contractRepository.Setup(c => c.SaveChanges());
            _contractRepository.Setup(c => c.GetById(1)).Returns(contract);
            //act
            var result = Assert.IsType<RedirectToActionResult>(_controller.Delete(1));
            //assert
            Assert.Equal("Index", result.ActionName);
            _contractRepository.Verify(c => c.Delete(contract),Times.Once);
            _contractRepository.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void Delete_ProductNotFound_ReturnsNotFound() 
        {
            _contractRepository.Setup(c => c.GetById(1)).Returns(null as Contract);
            var result = _controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_UnsuccessfullDelete_RedirectsToIndex() 
        {
            _contractRepository.Setup(c => c.GetById(1)).Throws<ArgumentException>();
            
            var result = Assert.IsType<RedirectToActionResult>(_controller.Delete(1));
            Assert.Equal("Index", result?.ActionName);
           // Assert.Equal("Sorry, something went wrong, Contract 1 was not deleted…", _controller.TempData["error"]);
        }


        #endregion
    }
}
