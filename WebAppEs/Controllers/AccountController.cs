using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.Models;
using WebAppEs.ViewModel.Authentication;

namespace WebAppEs.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    ApplicationUser appUser = await _userManager.FindByNameAsync(user.Username);
                    
                    var rolenameList = await _userManager.GetRolesAsync(appUser);
                    var rolename = rolenameList[0];


                    HttpContext.Session.SetString("EmployeeID", appUser.EmployeeID);
                    HttpContext.Session.SetString("Name", appUser.Name);
                    HttpContext.Session.SetString("Email", appUser.Email);
                    HttpContext.Session.SetString("Id", appUser.Id);
                    HttpContext.Session.SetString("Username", appUser.UserName);
                    HttpContext.Session.SetString("Role", rolename);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
