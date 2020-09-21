using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIdentity.Models;
using DotNetCoreIdentity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIdentity.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManage;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManage = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                {
                    UserName = userViewModel.Email, 
                    Email = userViewModel.Email, 
                    FirstName = userViewModel.FirstName, 
                    LastName = userViewModel.LastName 
                };

                var result = await _userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    await _signInManage.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error.Description);
                }
            }
            return View(userViewModel);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
               var result = await _signInManage.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempts");
            }
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> logout()        
        {

            await _signInManage.SignOutAsync();
            return RedirectToAction("Index", "Home");           
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]       
        public async Task<IActionResult> IsEmailExists(string email)
        {
          var user = await _userManager.FindByEmailAsync(email);
            if(user==null)
            {
                return Json(true);
            }
            else
            {
                return Json("Email already exists!");
            }
        }
    }
}
