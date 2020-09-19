using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIdentity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIdentity.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManage;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManage = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = userViewModel.Email, Email = userViewModel.Email };

                var result = await _userManager.CreateAsync(user, userViewModel.Password);

                if(result.Succeeded)
                {
                   await _signInManage.SignInAsync(user, isPersistent:false);
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors)
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
    }
}
