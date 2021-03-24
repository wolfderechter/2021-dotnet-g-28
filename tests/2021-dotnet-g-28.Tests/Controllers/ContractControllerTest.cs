using _2021_dotnet_g_28.Controllers;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Mock<IContractTypeRepository> _contractTypeRepository;
        private Mock<UserManager<IdentityUser>> _userManager;

        public ContractControllerTest()
        {
            _contractRepository = new Mock<IContractRepository>();
            _contractTypeRepository = new Mock<IContractTypeRepository>();
            _contactPersonRepository = new Mock<IContactPersonRepository>();
           // _userManager = new Mock<UserManager<IdentityUser>>();
            _controller = new ContractController(_contractRepository.Object, null, _contactPersonRepository.Object,_contractTypeRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }

        #region -- Index --
        [Fact]
        public void Index_PassesListOfContractsInViewResultModel()
        {
            ContractType contractType = new ContractType() { Name = "BusinessHours E-Mail", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email, MaxResponseTime = 24, IsOutsideBusinessHours = false, Price = 150, MinDuration = 1 };
            ContractType contractType2 = new ContractType() { Name = "Weekend E-Mail", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email, MaxResponseTime = 48, IsOutsideBusinessHours = true, Price = 2000, MinDuration = 2 };
            Contract contractRunning1 = new Contract() { StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(3), Status = ContractEnum.status.Running, Type = contractType };
            Contract contractRunning2 = new Contract() { StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(2), Status = ContractEnum.status.Ended, Type = contractType2 };
            List<Contract> contracts = new List<Contract>() { contractRunning1, contractRunning2 };
            _contractRepository.Setup(m => m.GetAll()).Returns(contracts);
            var result = Assert.IsType<ViewResult>(_controller.Index());
            var contractsInModel = Assert.IsType<List<Contract>>(result.Model);
            Assert.Equal(1, contractsInModel.Count);
            Assert.Equal(1, contractsInModel[0].ContractNr);
            Assert.Equal(2, contractsInModel[1].ContractNr);
        }
        #endregion

        #region DELETE TESTEN
        [Fact]
        public void Delete_ProductFound_DeletesContractAndRedirectsToIndex()
        {
            //arrange
            Contract contract = new Contract() { ContractNr = 1, Status = ContractEnum.status.InProgress };
            _contractRepository.Setup(c => c.Delete(It.IsAny<Contract>()));
            _contractRepository.Setup(c => c.SaveChanges());
            _contractRepository.Setup(c => c.GetById(1)).Returns(contract);
            //act
            var result = Assert.IsType<OkResult>(_controller.Delete(1));
            //assert
            _contractRepository.Verify(c => c.Delete(contract), Times.Once);
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
            _contractRepository.Setup(c => c.GetById(1)).Throws<Exception>();

            var result = Assert.IsType<OkResult>(_controller.Delete(1));
        }
        #endregion



        #region -- Create GET --
        [Fact]
        public void Create_PassesNewContractInContractCreateViewModelAndReturnsSelectListContractType()
        {
            ContractType contractType1 = new ContractType() { Name = "BusinessHours E-Mail", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email, MaxResponseTime = 24, IsOutsideBusinessHours = false, Price = 150, MinDuration = 1 };
            ContractType contractType2 = new ContractType() { Name = "Weekend E-Mail", IsActive = true, CreationMethod = ContractTypeEnum.CreationMethod.Email, MaxResponseTime = 48, IsOutsideBusinessHours = true, Price = 2000, MinDuration = 2 };
            List<ContractType> types = new List<ContractType>() { contractType1, contractType2 };
            _contractTypeRepository.Setup(m => m.GetAllActive()).Returns(types);
            var result = Assert.IsType<ViewResult>(_controller.Create());
            var contractTypesInViewData = Assert.IsType<SelectList>(result.ViewData["ContractTypeNames"]);
            var contractCreatevm = Assert.IsType<ContractCreateViewModel>(result.Model);
            Assert.Null(contractCreatevm.TypeName);
            Assert.Equal(2, contractTypesInViewData.Count());
            Assert.Null(contractTypesInViewData?.SelectedValue);
        }
        #endregion

        #region -- Create POST --
        [Fact]
        public void Create_ValidContract_CreatesAndPersistsContractAndRedirectsToActionIndex()
        {
            _contractRepository.Setup(m => m.Add(It.IsAny<Contract>()));
            var contractCvm = new ContractCreateViewModel()
            {
                duration=2,
                TypeName = "Weekend E-Mail"
            };
            var result = Assert.IsType<ViewResult>(_controller.Create(contractCvm));
            Assert.Equal("Create", result?.ViewName);
            _contractRepository.Verify(m => m.Add(It.IsAny<Contract>()), Times.Once());
            _contractRepository.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Create_InvalidBrewer_DoesNotCreateNorPersistsBrewerAndRedirectsToActionIndex()
        {
            _contractRepository.Setup(m => m.Add(It.IsAny<Contract>()));
            var contractCvm = new ContractCreateViewModel() { duration=4,TypeName= "Weekend E-Mail" };
            var result = Assert.IsType<ViewResult>(_controller.Create(contractCvm)); ;
            Assert.Equal("Create", result.ViewName);
            var contractTypesInViewData = Assert.IsType<SelectList>(result.ViewData["ContractTypeNames"]);

            _contractRepository.Verify(m => m.Add(It.IsAny<Contract>()), Times.Never());
            _contractRepository.Verify(m => m.SaveChanges(), Times.Never());
        }

        #endregion

        [Fact(Skip = "Navragen leerkracht meer informatie")]
        public void Index_PassedOrderedListOfContractsFromUsersCompanyInViewResultModel()
        {
            //fout ivm UserManager en andere 
            // arrange
            string userId = "1234";
            IdentityUser user = new IdentityUser() { Id = userId };
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(user);
            int companyNr = 12;
            ContactPerson person = new ContactPerson() { User = user, Company = new Company() { CompanyNr = companyNr } };
            _contactPersonRepository.Setup(c => c.getById(It.IsAny<String>())).Returns(person);
            Contract contract1 = new Contract() { ContractNr = 1, Company = new Company() { CompanyNr = companyNr } };
            Contract contract2 = new Contract() { ContractNr = 2, Company = new Company() { CompanyNr = companyNr } };
            List<Contract> contracts = new List<Contract>() { contract1, contract2 };
            _contractRepository.Setup(c => c.GetByIdAndStatusAndDuration(new List<ContractEnum.status>() { ContractEnum.status.InProgress, ContractEnum.status.Running }, new List<int> { 1, 2, 3 }, 12)).Returns(contracts);

            //act
            var result = Assert.IsType<ViewResult>(_controller.Index());
            //assert

        }
    }
}
