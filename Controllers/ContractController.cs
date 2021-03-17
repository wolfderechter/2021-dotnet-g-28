using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using _2021_dotnet_g_28.Models.Viewmodels;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2021_dotnet_g_28.Controllers
{
    [Authorize(Policy = "Customer")]
    public class ContractController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly IContractTypeRepository _contractTypeRepository;
        public ContractController(IContractRepository contractRepository, UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository, IContractTypeRepository contractTypeRepository)
        {
            _contractRepository = contractRepository;
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _contractTypeRepository = contractTypeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //gets the connected contactperson to the logged in user
            ContactPerson contactPerson = await GetLoggedInContactPerson();
            ContractIndexViewModel model = new ContractIndexViewModel();
            //inserts list of checkboxitems into indexViewModel and filter
            model.CheckBoxItems = new List<StatusModel>();
            foreach (ContractEnum.status contractStatus in Enum.GetValues(typeof(ContractEnum.status)))
            {
                model.CheckBoxItems.Add(new StatusModel() { Status = contractStatus, IsSelected = false });
            }
            model.CheckBoxItems.SingleOrDefault(c => c.Status == ContractEnum.status.InProgress).IsSelected = true;
            model.CheckBoxItems.SingleOrDefault(c => c.Status == ContractEnum.status.Running).IsSelected = true;
            //insert list of duurcheckbox items into model
            model.DuurCheckbox = new List<DuurModel>();
            for (int teller = 1; teller < 4; teller++)
            {
                model.DuurCheckbox.Add(new DuurModel() { Duration = teller, IsSelected = true });
            }
            model.Contracts = _contractRepository.GetByIdAndStatusAndDuration(new List<ContractEnum.status> { ContractEnum.status.InProgress, ContractEnum.status.Running }, new List<int> { 1, 2, 3 }, contactPerson.Company.CompanyNr);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ContractIndexViewModel model)
        {
            //getting contactperson from the signedin user
            ContactPerson contactPerson = await GetLoggedInContactPerson();
            //getting the selected statusses/
            List<ContractEnum.status> selectedStatusses = model.CheckBoxItems.Where(c => c.IsSelected).Select(c => c.Status).ToList();
            List<int> selectedDuration = model.DuurCheckbox.Where(c => c.IsSelected).Select(c => c.Duration).ToList();
            //getting contracts connected to statusses
            model.Contracts = _contractRepository.GetByIdAndStatusAndDuration(selectedStatusses, selectedDuration, contactPerson.Company.CompanyNr);
            return View(model);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var contractNr = id;
            try
            {
                Contract contract = _contractRepository.GetById(contractNr);
                if (contract == null)
                {
                    return NotFound();
                }
                _contractRepository.Delete(contract);
                _contractRepository.SaveChanges();
                TempData["message"] = $"Contract {contractNr} was sucessfully cancelled…";
            }
            catch
            {
                TempData["error"] = $"Sorry, something went wrong, Contract {contractNr} was not cancelled…";
            }
            return Ok();
        }

        public IActionResult Details(int contractNr)
        {
            Contract contract = _contractRepository.GetById(contractNr);
            return View(contract);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ContractCreateViewModel model = new ContractCreateViewModel();
            model.ContractTypes= _contractTypeRepository.GetAllActive();
            ViewData["ContractTypeNames"] = GetCategoriesSelectList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContractCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContactPerson contactPerson = await GetLoggedInContactPerson();
                    var contract = new Contract(_contractTypeRepository.GetByName(model.TypeName), model.duration, contactPerson.Company);
                    _contractRepository.Add(contract);
                    _contractRepository.SaveChanges();
                    TempData["message"] = $"You successfully created a new contract .";
                }
                catch (ArgumentException)
                {
                    TempData["error"] = "Sorry, you can only have one contract that's either running or in progress";
                    ViewData["ContractTypeNames"] = GetCategoriesSelectList();
                    model.ContractTypes = _contractTypeRepository.GetAllActive();
                    return View(model);
                }
                catch(Exception) 
                {
                    TempData["error"] = "Sorry, something went wrong, the Contract was not created...";
                    ViewData["ContractTypeNames"] = GetCategoriesSelectList();
                    model.ContractTypes = _contractTypeRepository.GetAllActive();
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractTypeNames"] = GetCategoriesSelectList();
            model.ContractTypes = _contractTypeRepository.GetAllActive();
            return View(model);
        }
        private SelectList GetCategoriesSelectList()
        {
            return new SelectList(_contractTypeRepository.GetAllActive().OrderBy(g => g.Name).ToList(),
                nameof(ContractType.Name), nameof(ContractType.Name));
        }

        private async Task<ContactPerson> GetLoggedInContactPerson() 
        {
            var user = await _userManager.GetUserAsync(User);
            return _contactPersonRepository.getById(user.Id);
        }
    }

}
