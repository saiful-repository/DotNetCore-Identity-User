using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIdentity.Models;
using DotNetCoreIdentity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCoreIdentity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministratorController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AdministratorController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: /<controller>/
        
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if(ModelState.IsValid)
            {
                var identityrole = new IdentityRole { Name = roleViewModel.RoleName };
                var result = await _roleManager.CreateAsync(identityrole);
                if (result.Succeeded)
                {                   
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleViewModel);
        }

        [HttpGet]
        public IActionResult ListRole()
        {
            var listRole =  _roleManager.Roles;
            return View(listRole);
        }

        [HttpGet]
        public async Task<IActionResult> RoleUser(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            List<RoleUserViewModel> model = new List<RoleUserViewModel>();

            if (role == null)
            {
                return NotFound();
            }

            foreach (var user in _userManager.Users)
            {
                var modelItem = new RoleUserViewModel();
                modelItem.RoleID = Id;
                modelItem.UserFullName = user.FirstName + ' ' + user.LastName;
                modelItem.UserID = user.Id;

                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    modelItem.IsSelected = true;
                }
                else
                {
                    modelItem.IsSelected = false;
                }

                model.Add(modelItem);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RoleUser(List<RoleUserViewModel> roleUserViewModels, string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }

            foreach (var item in roleUserViewModels)
            {
                var user = await _userManager.FindByIdAsync(item.UserID);
                if(item.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    var result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!item.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction("ListRole");
        }

    }
}
