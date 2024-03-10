using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.DAL.Models;
using Akalaat.Models;
using Akalaat.ViewModels;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Akalaat.Controllers;

public class AdminController : Controller
{
    private readonly IGenericRepository<Vendor>  _vendorRepository;

    private readonly RoleManager<IdentityRole> RoleManager;
    public UserManager<ApplicationUser> UserManager { get; }
    public SignInManager<ApplicationUser> SignInManager { get; }


    public AdminController(IGenericRepository<Vendor>  vendorRepository, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
        , RoleManager<IdentityRole> roleManager)
    {
        _vendorRepository = vendorRepository;
        UserManager = userManager;
        SignInManager = signInManager;
        RoleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Vendors  = await _vendorRepository.GetAllIncludingAsync(v=>v.Resturant);
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> AddVendor(RegisterViewModel newUserVM)
    {
        try
        {
            await _vendorRepository.BeginTransactionAsync();

            var vendor = await Register(newUserVM);
            if (vendor == null)
            {
                await _vendorRepository.RollbackTransactionAsync();
                return RedirectToAction("Index");
            }

            // Create a new Restaurant along with the Vendor
            var restaurant = new Resturant()
            {
                Name = "New Restaurant",
                Logo_URL = "https://media.istockphoto.com/id/1409329028/vector/no-picture-available-placeholder-thumbnail-icon-illustration-design.jpg?s=612x612&w=0&k=20&c=_zOuJu755g2eEUioiOUdz_mHKJQJn-tDgIAhQzyeKUQ=",
                Cover_URL = "https://media.istockphoto.com/id/1409329028/vector/no-picture-available-placeholder-thumbnail-icon-illustration-design.jpg?s=612x612&w=0&k=20&c=_zOuJu755g2eEUioiOUdz_mHKJQJn-tDgIAhQzyeKUQ=",
                Rating = 5,
                Vendor_ID = vendor.Id,
                Menu = new Menu()
            };

            // Add the Restaurant to the Vendor
            vendor.Resturant = restaurant;

            // Update the Vendor
            await _vendorRepository.Update(vendor);

            await _vendorRepository.ClearTrackingAsync();
        
            await _vendorRepository.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _vendorRepository.RollbackTransactionAsync();
            // Log the exception or handle it appropriately
        }
    
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteVendor(string vendorId)
    {
       
            try
            {
                await _vendorRepository.BeginTransactionAsync();

                var vendorToDelete = await _vendorRepository.GetByIdAsync(vendorId);
                if (vendorToDelete != null)
                {
                    await _vendorRepository.Delete(vendorToDelete.Id);
                }
                else
                {
                    throw new InvalidOperationException("Vendor not found.");
                }

                await _vendorRepository.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _vendorRepository.RollbackTransactionAsync();
            }
            
        
        return RedirectToAction("Index");
    }

    public async Task<Vendor?> Register(RegisterViewModel newUserVM, string Role = "Vendor")
    {
        //View("Error", new ErrorViewModel() { Message = "Error in Registration process", RequestId = "1001" })
        if (string.IsNullOrEmpty(Role)) return null;
        if (ModelState.IsValid)
        {
            Vendor userModel = new Vendor();
            userModel.UserName = newUserVM.UserName;
            userModel.Email = newUserVM.Email;


            var result = await UserManager.CreateAsync(userModel, newUserVM.Password);

            if (result.Succeeded)
            {

                var role = await RoleManager.FindByNameAsync(Role);
                if (role != null)
                {
                    var RoleAddRes = await UserManager.AddToRoleAsync(userModel, role.Name);
                    if (RoleAddRes.Succeeded)
                    {
                        //await SignInManager.SignInAsync(userModel, isPersistent: false);
                        return userModel;
                    }

                    await UserManager.DeleteAsync(userModel);
                    return null;
                }
                else
                {
                    await UserManager.DeleteAsync(userModel);
                    return null;
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

        return null;
    }
}


    
