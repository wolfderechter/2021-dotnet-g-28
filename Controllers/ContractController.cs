using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _2021_dotnet_g_28.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using _2021_dotnet_g_28.Models.Viewmodels;
using System;

namespace _2021_dotnet_g_28.Controllers
{
    [Authorize(Policy = "Customer")]
    public class ContractController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        public ContractController(IContractRepository contractRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository)
        {
            _contractRepository = contractRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //gets the connected contactperson to the logged in user
            var user = await _userManager.GetUserAsync(User);
            ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);
            IndexViewModel model = new IndexViewModel();
            //inserts list of checkboxitems into indexViewModel and filter
            model.CheckBoxItems = new List<StatusModel>();
            foreach (ContractEnum.status contractStatus in Enum.GetValues(typeof(ContractEnum.status)))
            {
                model.CheckBoxItems.Add(new StatusModel() { Status = contractStatus, IsSelected = false});
            }
            model.CheckBoxItems.SingleOrDefault(c => c.Status == ContractEnum.status.InProgress).IsSelected = true;
            model.CheckBoxItems.SingleOrDefault(c => c.Status == ContractEnum.status.Running).IsSelected = true;
            //insert list of duurcheckbox items into model
            model.DuurCheckbox = new List<DuurModel>();
            for (int teller = 1; teller < 4; teller++)
            {
                model.DuurCheckbox.Add(new DuurModel() { Duration = teller, IsSelected = true });
            }
            model.Contracts = _contractRepository.GetByIdAndStatusAndDuration(new List<ContractEnum.status> { ContractEnum.status.InProgress, ContractEnum.status.Running }, new List<int> { 1, 2,3 }, contactPerson.Company.CompanyNr);
            return View(model);
        }

        [HttpPost]
        public  async Task<ActionResult> Index(IndexViewModel model)
        {
            //getting contactperson from the signedin user
            var user = await _userManager.GetUserAsync(User);
            ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);
            //getting the selected statusses/
            List<ContractEnum.status> selectedStatusses = model.CheckBoxItems.Where(c => c.IsSelected).Select(c => c.Status).ToList();
            List<int> selectedDuration = model.DuurCheckbox.Where(c => c.IsSelected).Select(c => c.Duration).ToList();
            //getting contracts connected to statusses
            model.Contracts = _contractRepository.GetByIdAndStatusAndDuration(selectedStatusses,selectedDuration, contactPerson.Company.CompanyNr);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int contractNr) 
        {
            try
            {
                Contract contract = _contractRepository.GetById(contractNr);
                if (contract == null)
                {
                    return NotFound();
                }
                _contractRepository.Delete(contract);
                _contractRepository.SaveChanges();
                TempData["message"] = $"Contract {contractNr} was sucessfully deleted…";
            }
            catch 
            {
                TempData["error"] = $"Sorry, something went wrong, Contract {contractNr} was not deleted…";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int contractNr) 
        {
            Contract contract = _contractRepository.GetById(contractNr);
            return View(contract);
        }

    }

}
