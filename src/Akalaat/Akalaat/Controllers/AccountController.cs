using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                IdentityResult temp;
                ApplicationUser user;
                if(Role=="Customer")
                {
                    user = new Customer()
                    {
                        UserName = newUserVM.UserName,
                        Email = newUserVM.Email,
                        ShoppingCart = new ShoppingCart()
                    };
                    
                }
                else
                {
                    user = new Vendor()
                    {
                        UserName = newUserVM.UserName,
                        Email = newUserVM.Email
                    };
                }

                temp = await UserManager.CreateAsync(user, newUserVM.Password);

                if (temp.Succeeded)
                {
                    
                    var role = await roleManager.FindByNameAsync(Role);
                    if (role != null)
                    {
                        var RoleAddRes = await UserManager.AddToRoleAsync(user, role.Name);
                        if (RoleAddRes.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Login", "Account");
                        }
                        await UserManager.DeleteAsync(user);
                        return View("Error", new ErrorViewModel() { Message = "Error in Registration process please register again", RequestId = "1001" });
                    }
                    else
                    {
                        await UserManager.DeleteAsync(user);
                        return View("Error", new ErrorViewModel() { Message = "Error in Registration process please register again", RequestId = "1001" });
                    }
                }
                else
                {
                    foreach (var errorItem in temp.Errors)
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

        [Authorize]
        public IActionResult ChangeEmail()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel emailViewModel)
        {
            var user = await UserManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            var userWithTheSameEmail = await UserManager.FindByEmailAsync(emailViewModel.Email);
            if(userWithTheSameEmail != null) 
            { return View("Error", new ErrorViewModel() { Message = "This Email already exists.", RequestId = "1001" }); }

            user.Email=emailViewModel.Email;
            var result = await UserManager.UpdateAsync(user);
            if(!result.Succeeded)
                return View("Error", new ErrorViewModel() { Message = "This Email already exists.", RequestId = "1001" });

            await SignInManager.SignInAsync(user,false);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordVM)
        {
            var currentUser = await UserManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            var valid=await UserManager.CheckPasswordAsync(currentUser, changePasswordVM.CurrentPassword);
            if (!valid)
            {
                ModelState.AddModelError("", "Wrong password !");
            }
            var result= await UserManager.ChangePasswordAsync(currentUser, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong password !");
                return View(changePasswordVM);
            }
                
            return RedirectToAction("Index", "Home");
        }

        #region future consideration
        //private async Task UpdateEmailClaimInCookie(string newEmail) 
        //{
        //    var claimsPrincipal = User..Claims. as ClaimsIdentity;
        //    if (claimsPrincipal != null)
        //    {
        //        var claims = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Email).ToList();
        //        if (claims.Count > 0)
        //        {
        //            claims[0] = new Claim(ClaimTypes.Email, newEmail, claims[0].ValueType, claims[0].Issuer);
        //            await HttpContext.SignInAsync(claimsPrincipal); // Update the cookie
        //        }
        //    }
        //}
        #endregion
    }
}
