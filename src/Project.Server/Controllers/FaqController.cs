using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _2021_dotnet_g_28.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqRepository _faqRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISupportManagerRepository _supportManagerRepository;
        private readonly IContactPersonRepository _contactPersonRepository;

        public FaqController(IFaqRepository faqRepository, UserManager<IdentityUser> userManager, ISupportManagerRepository supportManagerRepository, IContactPersonRepository contactPersonRepository)
        {
            _faqRepository = faqRepository;
            _userManager = userManager;
            _supportManagerRepository = supportManagerRepository;
            _contactPersonRepository = contactPersonRepository;

        }
        public async Task<IActionResult> Index(string searchstring)
        {
            //get signed in user
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SupportManager"))
            {
                //get support manager connected 
                var supManager = _supportManagerRepository.GetById(user.Id);
                ViewData["GebruikersNaam"] = supManager.FirstName + ' ' + supManager.LastName;
            }
            else
            {
                //get contactperson matching with signed in user
                ContactPerson contactPerson = _contactPersonRepository.getById(user.Id);
                ViewData["GebruikersNaam"] = contactPerson.FirstName + ' ' + contactPerson.LastName;
                ViewData["Notifications"] = contactPerson.Notifications.Where(n => !n.IsRead).ToList();
            }

            ViewData["CurrentFilter"] = searchstring;
            var faqs = _faqRepository.GetAll();
            FaqIndexViewModel model = new FaqIndexViewModel();
        
            if(!String.IsNullOrEmpty(searchstring))
            {
                model.Faqs = _faqRepository.GetBySearchstring(searchstring);
            } else
            {
                model.Faqs = _faqRepository.GetAll();
            }

            return View(model);
        }
    }
}
