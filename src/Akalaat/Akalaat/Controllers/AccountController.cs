using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Akalaat.BLL.Models;
using Akalaat.BLL.Repositories;

namespace Akalaat.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AccountRepository _accountRepository;
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager, AccountRepository accountRepository)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            this.roleManager = roleManager;
            _accountRepository = accountRepository;
        }


        public IActionResult Register(string Role)
        {
            if (string.IsNullOrEmpty(Role))
                return View("Error",
                    new ErrorViewModel() { Message = "Error in Registration process", RequestId = "1001" });

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
        [HttpPost] //this action for Vendor and customer registration only
        public async Task<IActionResult> Register([FromQuery] string Role, RegisterViewModel newUserVM)
        {
            if (string.IsNullOrEmpty(Role))
                return View("Error",
                    new ErrorViewModel() { Message = "Error in Registration process", RequestId = "1001" });
            if (ModelState.IsValid)
            {
                IdentityResult temp;
                ApplicationUser user;
                if (Role == "Customer")
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
                            return RedirectToAction("Home", "Restaurant");
                        }

                        await UserManager.DeleteAsync(user);
                        return View("Error",
                            new ErrorViewModel()
                            {
                                Message = "Error in Registration process please register again", RequestId = "1001"
                            });
                    }
                    else
                    {
                        await UserManager.DeleteAsync(user);
                        return View("Error",
                            new ErrorViewModel()
                            {
                                Message = "Error in Registration process please register again", RequestId = "1001"
                            });
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

                        return RedirectToAction("Index", "Home");
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
            if (userWithTheSameEmail != null)
            {
                return View("Error",
                    new ErrorViewModel() { Message = "This Email already exists.", RequestId = "1001" });
            }

            user.Email = emailViewModel.Email;
            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
                return View("Error",
                    new ErrorViewModel() { Message = "This Email already exists.", RequestId = "1001" });

            await SignInManager.SignInAsync(user, false);
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

            var valid = await UserManager.CheckPasswordAsync(currentUser, changePasswordVM.CurrentPassword);
            if (!valid)
            {
                ModelState.AddModelError("", "Wrong password !");
            }

            var result = await UserManager.ChangePasswordAsync(currentUser, changePasswordVM.CurrentPassword,
                changePasswordVM.NewPassword);
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

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult ExternalLogin(string provider, string? returnURL = null)
        {
            var redirectURL = Url.Action("RegisterExternalUser", values: new { returnURL });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectURL);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterExternalUser(string? returnURL = null,
            string? remoteError = null)
        {
            returnURL = returnURL ?? Url.Content("~/");
            var message = "";

            if (remoteError != null)
            {
                message = $"Error from external provider: {remoteError}";
                return RedirectToAction("login", routeValues: new { message });
            }

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                message = "Error loading external login information.";
                return RedirectToAction("login", routeValues: new { message });
            }

            var externalLoginResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);

            // The account already exists
            if (externalLoginResult.Succeeded)
            {
                return LocalRedirect(returnURL);
            }

            string email = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            }
            else
            {
                message = "Error while reading the email from the provider.";
                return RedirectToAction("login", routeValues: new { message });
            }

            var usuario = new Customer() { Email = email, UserName = email, ShoppingCart = new ShoppingCart() };

            var createUserResult = await UserManager.CreateAsync(usuario);
            if (!createUserResult.Succeeded)
            {
                message = createUserResult.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { message });
            }

            var addLoginResult = await UserManager.AddLoginAsync(usuario, info);

            if (addLoginResult.Succeeded)
            {
                await SignInManager.SignInAsync(usuario, isPersistent: false, info.LoginProvider);
                return LocalRedirect(returnURL);
            }

            message = "There was an error while logging you in.";
            return RedirectToAction("login", routeValues: new { message });
        }


        [AllowAnonymous, HttpGet("forget-password")]
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordVM() );
        }

        [AllowAnonymous, HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                }

                ModelState.Clear();
                model.EmailSent = true;
            }

            return View(model);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}
    

    



