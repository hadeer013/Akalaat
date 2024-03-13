using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.AddressBookSpec;
using Akalaat.BLL.Specifications.EntitySpecs.BranchSpec;
using Akalaat.DAL.Models;
using Akalaat.Helper;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akalaat.Controllers
{
    public class BranchController : Controller
    {
        private readonly IGenericRepository<Resturant> resturantRepo;
        private readonly IGenericRepository<Branch> branchRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepository<Address_Book> addressRepo;
        private readonly IGenericRepository<City> cityRepo;

        public BranchController(IGenericRepository<Resturant> resturantRepo,IGenericRepository<Branch> branchRepo,
            UserManager<ApplicationUser> userManager,IGenericRepository<Address_Book> addressRepo,IGenericRepository<City>cityRepo)
        {
            this.resturantRepo = resturantRepo;
            this.branchRepo = branchRepo;
            this.userManager = userManager;
            this.addressRepo = addressRepo;
            this.cityRepo = cityRepo;
        }
        //this needs login or not 
        [AllowAnonymous]
        public async Task<IActionResult> Index(int Id, string Name = "") //ResturantId
        {
            var branchSpec = new BranchwithResturantSpecification(Id, Name);
            var branches = await branchRepo.GetAllWithSpec(branchSpec);


            bool deliverToYou = false;
            var ListVM = new List<viewBranchVM>();

            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                var spec = new AddresswithRegionSpec(user.Id);
                var userAddress = await addressRepo.GetByIdWithSpec(spec);
                if (userAddress != null)
                {
                    foreach (var item in branches)
                    {
                        var branchVM = new viewBranchVM()
                        {
                            AddressDetails = item.AddressDetails,
                            CloseHour = item.Close_Hour,
                            OpenHour = item.Open_Hour,
                            Id = item.Id,
                            RegionName = item.Region.Name,
                            DeliveryState = DeliverToYou.NotSpecified,
                            IsAuthenticated = true,
                            DeliveringAreas = item?.DeliveryAreas?.Select(d => d.Name).ToList()
                        };
                        ListVM.Add(branchVM);

                    }
                }
            }
            else
            {

            }

            return View(ListVM);
        }


        public async Task<IActionResult> AddBranch(int Id)
        {
            var Resurant = await resturantRepo.GetByIdAsync(Id);
            if (Resurant == null) return BadRequest();

            ViewBag.Cities = await cityRepo.GetAllAsync();
            return View();
        }

        public async Task<IActionResult> AddBranch(AddBranchVM addBranchVM)
        {
            if (ModelState.IsValid)
            {
                var Resurant = await resturantRepo.GetByIdAsync(addBranchVM.ResturantId);
                if (Resurant == null) return BadRequest();

                var branch = new Branch()
                {
                    AddressDetails = addBranchVM.AddressDetails,
                    Close_Hour = addBranchVM.CloseHour,
                    Open_Hour = addBranchVM.OpenHour,
                    Region_ID = addBranchVM.RegionId,
                    Resturant_ID = addBranchVM.ResturantId,
                    Estimated_Delivery_Time = 15,
                    IsDelivery = true,
                    IsDineIn = true
                };

                await branchRepo.Add(branch);
                return RedirectToAction("Index");   

            }
            else
                return View(addBranchVM);
        }

        public async Task<IActionResult> GetBranchDeliveringArea(int Id)  //BranchId
        {
            var spec = new BranchwithResturantSpecification(Id);
            var branch = await branchRepo.GetByIdWithSpec(spec);

            return View(branch.DeliveryAreas.Select(d=>d.Name).ToList()); 
        }

    }
}
