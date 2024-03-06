using Akalaat.DAL.Models;
using Akalaat.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            ,RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this.roleManager = roleManager;
        }


        public IActionResult Register(string Role)
        { 
            if(string.IsNullOrEmpty(Role)) 
                return View("Error", new ErrorViewModel() { Message = "Error in Registration process", RequestId = "1001" });

            ViewBag.Role = Role;
            return View();
        }

        [AllowAnonymous]
        public IActionResult RegisterAsCustomer()
        {
            return RedirectToAction("Register", "Account", new { Role = "Customer" });
        }

        [AllowAnonymous]
        public IActionResult RegisterAsVendor()
        {
            return RedirectToAction("Register", "Account", new { Role = "Vendor" });
        }


        [AllowAnonymous]
        [HttpPost]  //this action for Vendor and customer registration only
        public async Task<IActionResult> Register([FromQuery]string Role,RegisterViewModel newUserVM)
        {
            if (string.IsNullOrEmpty(Role)) return View("Error", new ErrorViewModel() { Message = "Error in Registration process", RequestId = "1001" });
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName = newUserVM.UserName;
                userModel.Email = newUserVM.Email;

                
                var result = await UserManager.CreateAsync(userModel, newUserVM.Password);

                if (result.Succeeded)
                {
                    
                    var role = await roleManager.FindByNameAsync(Role);
                    if (role != null)
                    {
                        var RoleAddRes = await UserManager.AddToRoleAsync(userModel, role.Name);
                        if (RoleAddRes.Succeeded)
                        {
                            await SignInManager.SignInAsync(userModel, isPersistent: false);
                            return RedirectToAction("Login", "Account");
                        }
                        await UserManager.DeleteAsync(userModel);
                        return View("Error", new ErrorViewModel() { Message = "Error in Registration process please register again", RequestId = "1001" });
                    }
                    else
                    {
                        await UserManager.DeleteAsync(userModel);
                        return View("Error", new ErrorViewModel() { Message = "Error in Registration process please register again", RequestId = "1001" });
                    }
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("", errorItem.Description);
                    }
                }
            }

            return View(newUserVM);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel UserVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModelFromDb = await UserManager.FindByEmailAsync(UserVM.Email);

                if (userModelFromDb != null)
                {
                    bool found = await UserManager.CheckPasswordAsync(userModelFromDb, UserVM.Password);
                    if (found)
                    {
                        await SignInManager.SignInAsync(userModelFromDb, UserVM.RememberMe);

                        return RedirectToAction("Index","Home");
                    }
                }
            }
            ModelState.AddModelError("", "Wrong UserName Or Password!!");
            return View(UserVM);
        }
    }
}
