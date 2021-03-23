using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021_dotnet_g_28.Models.Domain;
using _2021_dotnet_g_28.Models.Viewmodels;
using Microsoft.AspNetCore.Mvc;

namespace _2021_dotnet_g_28.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqRepository _faqRepository;
        public FaqController(IFaqRepository faqRepository)
        {
            _faqRepository = faqRepository;

        }
        public async Task<IActionResult> Index(string searchstring)
        {
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
