using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.BLL.Specifications.EntitySpecs.AddressBookSpec;
using Akalaat.BLL.Specifications.EntitySpecs.CitySpec;
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
        private readonly IGenericRepository<City> cityRepo;
        private readonly IDistrictRepository districtRepo;
        private readonly IRegionRepository regionRepo;

        public AddressBookController(IGenericRepository<Address_Book> addressRepo,UserManager<ApplicationUser> userManager,
            IGenericRepository<City> cityRepo,IDistrictRepository districtRepo,IRegionRepository regionRepo)
        {
            this.addressRepo = addressRepo;
            this.userManager = userManager;
            this.cityRepo = cityRepo;
            this.districtRepo = districtRepo;
            this.regionRepo = regionRepo;
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var customer = user as Customer;
            var RegSpec = new AddresswithRegionSpec(customer.Id);
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
            ViewBag.Cities=await cityRepo.GetAllAsync();

            return View();
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddAddressBook(AddAddressBookVM addressBookVM)
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
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var customer = user as Customer;
            var RegSpec = new AddresswithRegionSpec(customer.Id);
            var AddressBook = await addressRepo.GetByIdWithSpec(RegSpec);

            ViewBag.Cities = await cityRepo.GetAllAsync();
            ViewBag.Districts = await districtRepo.GetAllDistrictsByCityId(AddressBook.Region.District.City_ID);
            ViewBag.Regions = await regionRepo.GetAllRegionsByDistrictId(AddressBook.Region.District_ID);

            var editAddVM = new EditAddressBookVM()
            {
                Id = Id,
                AddressDetails = AddressBook.AddressDetails,
                CityId = AddressBook.Region.District.City_ID,
                DistrictId = AddressBook.Region.District_ID,
                RegionId = AddressBook.Region_ID
            };

            return View(editAddVM);
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Edit(int Id, EditAddressBookVM editAddressBook)
        {
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var customer = user as Customer;
            var RegSpec = new AddresswithRegionSpec(customer.Id);
            var AddressBook = await addressRepo.GetByIdWithSpec(RegSpec);

            AddressBook.Region_ID = editAddressBook.RegionId;
            AddressBook.AddressDetails = editAddressBook.AddressDetails;

            await addressRepo.Update(AddressBook);

            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Delete(int Id) //????
        //{
        //    var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        //    var customer = user as Customer;
        //    var address = await addressRepo.GetByIdAsync(customer.Id);
        //    await addressRepo.Delete(address);
        //    return RedirectToAction(nameof(Index));
        //}

    }
}

