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

        public StatisticController(UserManager<IdentityUser> userManager, IContactPersonRepository contactPersonRepository)
        {
            _userManager = userManager;
            _contactPersonRepository = contactPersonRepository;
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
            } else
            {
                //give and empty string to viewdata -> filter won't filter
                //support manager sees statistics from all companies combined
                ViewData["CompanyNr"] = "";
            }

            return View();
        }
    }
}
