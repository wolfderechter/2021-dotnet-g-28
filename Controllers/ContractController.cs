using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _2021_dotnet_g_28.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace _2021_dotnet_g_28.Controllers
{
    [Authorize(Policy ="Customer")]
    public class ContractController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        public ContractController(IContractRepository contractRepository,ApplicationDbContext dbContext,UserManager<IdentityUser> userManager,IContactPersonRepository contactPersonRepository)
        {
            _contractRepository = contractRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ContactPerson contact = _contactPersonRepository.getById(user.Id);
            return View(_contractRepository.GetByIdAndStatus(new List<ContractEnum.status> { ContractEnum.status.InProgress, ContractEnum.status.Running }, contact.Company.CompanyNr));
        }
       
        public IActionResult Delete(int contractNr) 
        {
            try
            {
                Contract contract = _contractRepository.GetById(contractNr);
                _contractRepository.Delete(contract);
                
            }
            catch 
            {
                TempData["error"] = $"Sorry, something went wrong, Contract  {contractNr} was not deleted…";
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
