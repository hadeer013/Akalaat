using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akalaat.Controllers
{
    public class AddressBookController : Controller
    {
        private readonly IGenericRepository<Address_Book> addressRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public AddressBookController(IGenericRepository<Address_Book> addressRepo,UserManager<ApplicationUser> userManager)
        {
            this.addressRepo = addressRepo;
            this.userManager = userManager;
        }

        //[Authorize(Roles ="Customer")]
        //public async Task<IActionResult> Index()
        //{
        //    var userId = (await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email))).Id;
        //    //var address=await addressRepo.get
        //}


        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> AddAddressBook()
        {
            return View();
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddAddressBook(AddressBookVM addressBookVM)
        {
            if (ModelState.IsValid)
            {
                var customer = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

                var addressBook = new Address_Book()
                {
                    AddressDetails = addressBookVM.AddressDetails,
                    Customer_ID = customer.Id,
                    Region_ID = addressBookVM.Region_ID
                };

                await addressRepo.Add(addressBook);
                return RedirectToAction("Index");
            }
            return View(addressBookVM);
        }
    }
}
