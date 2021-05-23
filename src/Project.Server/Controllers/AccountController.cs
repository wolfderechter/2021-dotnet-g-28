using _2021_dotnet_g_28.Models.viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHtmlLocalizer<AccountController> _localizor;

        public AccountController(SignInManager<IdentityUser> signInm, UserManager<IdentityUser> userm, IHtmlLocalizer<AccountController> localizor)
        {
            this.signInManager = signInm;
            _userManager = userm;
            _localizor = localizor;
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                 var result = await signInManager.PasswordSignInAsync(
                 model.Username, model.Password, model.RememberMe,true);

                if (result.Succeeded)
                {
                    TempData["username"] = model.Username;
                    return RedirectToAction("Index", "Ticket");
                }
                if (result.IsLockedOut) {
                    ModelState.AddModelError(string.Empty, "Your account has been locked, please contact support for more information : support.sce@actemium.com ");
                    return View(model);
                }


                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");  
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("Account/IsValidUserJava/{username}/{password}")]
        public async Task<string> IsValidUserJava(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                return "true";
            } else
            {
                return "false";
            }

            return "verkeerde inloggegevens";
        }

        [HttpGet]
        [Route("Account/CreateUserJava/{username}/{email}/{phonenumber}/{role}")]
        public async Task<string> CreateUserJava(string username, string email, string phoneNumber, string role)
        {
            IdentityUser user = new IdentityUser { UserName = username, Email = email, PhoneNumber = phoneNumber};
            await _userManager.CreateAsync(user, "Paswoord_1");
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
            return "true";

        }

        [HttpGet]
        [Route("Account/getUserDataJava")]
        public async Task<string> GetUserDataJava()
        {
            //get signed in user
            var user = await _userManager.GetUserAsync(User);
            return JsonConvert.SerializeObject(user);
        }
    }
}
