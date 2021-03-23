﻿using _2021_dotnet_g_28.Models.viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IHtmlLocalizer<AccountController> _localizor;

        public AccountController(SignInManager<IdentityUser> signInm, IHtmlLocalizer<AccountController> localizor)
        {
            this.signInManager = signInm;
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
    }
}