using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.AddressBookSpec;
using Akalaat.BLL.Specifications.EntitySpecs.RegionSpec;
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

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var customer = user as Customer;
            var RegSpec = new AddresswithRegionSpec(customer.Address_Book_ID);
            var AddressBook=await addressRepo.GetByIdWithSpec(RegSpec);
            DisplayAddressBookVM MappedaddressBook = null;
            if (AddressBook!=null)
            {
                MappedaddressBook = new DisplayAddressBookVM()
                {
                    AddressBookId = AddressBook.Id,
                    CityName = AddressBook.Region.District.City.Name,
                    DistrictName = AddressBook.Region.District.Name,
                    RegionName = AddressBook.Region.Name
                };

                return View(MappedaddressBook);
            }
            return View(MappedaddressBook);
        }


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

        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> Edit(int Id)
        {
            return View();
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Edit(int Id, EditAddressBookVM editAddressBook)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var customer = user as Customer;
            var RegSpec = new AddresswithRegionSpec(customer.Address_Book_ID);
            var AddressBook = await addressRepo.GetByIdWithSpec(RegSpec);

            AddressBook.Region_ID = editAddressBook.RegionId;
            AddressBook.Region.District_ID = editAddressBook.DistrictId;
            AddressBook.Region.District.City_ID = editAddressBook.CityId;

            await addressRepo.Update(AddressBook);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var customer = user as Customer;

            var address = await addressRepo.GetByIdAsync(customer.Address_Book_ID);
            await addressRepo.Delete(address);

            return RedirectToAction(nameof(Index));
        }

    }
}

