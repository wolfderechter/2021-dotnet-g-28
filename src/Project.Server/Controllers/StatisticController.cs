using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Data.Repositories;
using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _2021_dotnet_g_28.Controllers
{
    public class StatisticController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IContactPersonRepository _contactPersonRepository;
        private readonly ISupportManagerRepository _supportManagerRepository;

        public StatisticController(UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository, ISupportManagerRepository supportManagerRepository)
        {
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
            _supportManagerRepository = supportManagerRepository;
        }

        public async Task<IActionResult> Index()
        {
            //get signed in user
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Customer"))
            {
                //get company from signed in contactperson
                ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);
                Company company = contactPerson.Company;
                //give companynr to viewdata to display company specific statistics
                ViewData["CompanyNr"] = company.CompanyNr;
                ViewData["GebruikersNaam"] = contactPerson.FirstName + ' ' + contactPerson.LastName;
                ViewData["Notifications"] = contactPerson.Notifications.Where(n => !n.IsRead).ToList();
                ViewData["isSupportManager"] = false;
            } else
            {
                //give and empty string to viewdata -> filter won't filter
                //support manager sees statistics from all companies combined
                ViewData["CompanyNr"] = "";
                var supManager = _supportManagerRepository.GetById(user.Id);
                ViewData["GebruikersNaam"] = supManager.FirstName + ' ' + supManager.LastName;
                ViewData["isSupportManager"] = true;
            }

            return View();
        }
    }
}
