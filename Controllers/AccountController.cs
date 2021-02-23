﻿using _2021_dotnet_g_28.Models.viewmodels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(SignInManager<IdentityUser> signInm)
        {
            this.signInManager = signInm;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Login();
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
                    
                    return RedirectToAction("index", "home");
                }
                if (result.IsLockedOut) {
                    ModelState.AddModelError(string.Empty, "je account is gelocked door teveel foute pogingen je moet 5 minuten wachten");
                    return View(model);
                }


                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");  
            }

            return View(model);
        }
    }
}
