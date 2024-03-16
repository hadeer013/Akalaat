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
        private readonly IRegionRepository regionRepo;
        private readonly IBranchDeliveryRepository branchDeliveryRepo;
        private readonly IDistrictRepository districtRepo;

        public BranchController(IGenericRepository<Resturant> resturantRepo, IGenericRepository<Branch> branchRepo,
            UserManager<ApplicationUser> userManager, IGenericRepository<Address_Book> addressRepo, IGenericRepository<City> cityRepo,
            IRegionRepository regionRepo, IBranchDeliveryRepository branchDeliveryRepo, IDistrictRepository districtRepo)
        {
            this.resturantRepo = resturantRepo;
            this.branchRepo = branchRepo;
            this.userManager = userManager;
            this.addressRepo = addressRepo;
            this.cityRepo = cityRepo;
            this.regionRepo = regionRepo;
            this.branchDeliveryRepo = branchDeliveryRepo;
            this.districtRepo = districtRepo;
        }


        //this needs login or not 
        [AllowAnonymous]
        public async Task<IActionResult> Index(int Id, string Name = "") //ResturantId
        {
            var branchSpec = new BranchwithResturantSpecification(Id, Name);
            var branches = await branchRepo.GetAllWithSpec(branchSpec);

            int cityId = 0, districtId = 0, regionId = 0;

            var cityResult = int.TryParse(HttpContext.Session.GetString("CityId"), out cityId);
            var districtResult = int.TryParse(HttpContext.Session.GetString("DistrictId"), out districtId);
            var regionResult = int.TryParse(HttpContext.Session.GetString("RegionId"), out regionId);

            var locationProvided = false;

            if (cityResult && districtResult && regionResult)
                locationProvided = true;


            var ListVM = new List<viewBranchVM>();

            foreach (var item in branches)
            {
                var branchVM = new viewBranchVM()
                {
                    ResturantId = Id,
                    AddressDetails = item.AddressDetails,
                    CloseHour = item.Close_Hour,
                    OpenHour = item.Open_Hour,
                    Id = item.Id,
                    RegionName = item.Region.Name,
                    DeliveryState = locationProvided ? await CheckDeliverToLocation(regionId, item.Id) : DeliverToYou.NoDeliver,
                    LocationProvided = locationProvided,
                    DeliveringAreas = item?.DeliveryAreas?.Select(d => d.Name).ToList()
                };
                ListVM.Add(branchVM);

            }

            ViewBag.ResturantId = Id;

            return View(ListVM);
        }

        private async Task<DeliverToYou> CheckDeliverToLocation(int UserRegionId, int BranchId)
        {
            var spec = new BranchwithResturantSpecification(BranchId);
            var branch = await branchRepo.GetByIdWithSpec(spec);

            if (branch.DeliveryAreas.Select(d => d.Id).Contains(UserRegionId))
                return DeliverToYou.Deliver;
            return DeliverToYou.NoDeliver;
        }
        public async Task<IActionResult> AddBranch(int Id)
        {
            var Resurant = await resturantRepo.GetByIdAsync(Id);
            if (Resurant == null) return BadRequest();

            ViewBag.Cities = await cityRepo.GetAllAsync();
            ViewBag.ResId = Id;
            return View();
        }

        [HttpPost]
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
                    IsDelivery = addBranchVM.IsDelivery,
                    IsDineIn = addBranchVM.IsDineIn
                };
                await branchRepo.Add(branch);
                return RedirectToAction("Index", "Branch", new { Id = Resurant.Id });
            }
            else
            {
                ViewBag.Cities = await cityRepo.GetAllAsync();
                ViewBag.ResId = addBranchVM.ResturantId;
                return View(addBranchVM);
            }

        }
        public async Task<IActionResult> EditBranch(int Id)
        {
            var spec = new BranchwithResturantSpecification(Id);
            var branch = await branchRepo.GetByIdWithSpec(spec);
            if (branch == null) return NotFound();

            ViewBag.Cities = await cityRepo.GetAllAsync();
            ViewBag.Districts = await districtRepo.GetAllDistrictsByCityId(branch.Region.District.City_ID);
            ViewBag.Regions = await regionRepo.GetAllRegionsByDistrictId(branch.Region.District_ID);

            var EditVM = new EditBranchVM()
            {
                AddressDetails = branch.AddressDetails,
                CloseHour = branch.Close_Hour,
                OpenHour = branch.Open_Hour,
                IsDelivery = branch.IsDelivery,
                IsDineIn = branch.IsDineIn,
                BranchId = Id,
                RegionId = branch.Region_ID,
                CityId = branch.Region.District.City_ID,
                DistrictId = branch.Region.District_ID
            };

            return View(EditVM);
        }


        [HttpPost]
        public async Task<IActionResult> EditBranch(EditBranchVM editBranchVM)
        {
            if (ModelState.IsValid)
            {
                var branch = await branchRepo.GetByIdAsync(editBranchVM.BranchId);
                if (branch == null) return NotFound();

                branch.Open_Hour = editBranchVM.OpenHour;
                branch.AddressDetails = editBranchVM.AddressDetails;
                branch.Close_Hour = editBranchVM.CloseHour;
                branch.IsDelivery = editBranchVM.IsDelivery;
                branch.IsDineIn = editBranchVM.IsDineIn;
                branch.Region_ID = editBranchVM.RegionId;

                await branchRepo.Update(branch);
                return RedirectToAction("Index");

            }
            ViewBag.Cities = await cityRepo.GetAllAsync();
            ViewBag.Districts = await districtRepo.GetAllDistrictsByCityId(editBranchVM.CityId);
            ViewBag.Regions = await regionRepo.GetAllRegionsByDistrictId(editBranchVM.DistrictId);

            return View(editBranchVM);
        }




        public async Task<IActionResult> DeleteBranch(int Id) // NOT TESTED YET
        {
            var branch = await branchRepo.GetByIdAsync(Id);
            if (branch == null) return NotFound();

            try
            {
                await branchRepo.Delete<int>(branch.Id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { Message = ex.Message });
            }
        }
        //Vendor Role
        public async Task<IActionResult> AddBranchDeliveryAreas(int Id)//BranchId
        {
            var branch = await branchRepo.GetByIdAsync(Id);
            if (branch == null) return BadRequest();

            ViewBag.Cities = await cityRepo.GetAllAsync();
            ViewBag.BranchId = Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBranchDeliveryAreas(AddBranchDeliveryArea deliveryAreaVM)//BranchId
        {
            if (ModelState.IsValid)
            {
                var branch = await branchRepo.GetByIdAsync(deliveryAreaVM.BranchId);
                if (branch == null) return BadRequest();


                var region = await regionRepo.GetByIdAsync(deliveryAreaVM.RegionId);
                if (region == null) return BadRequest();


                var temp = new Available_Delivery_Area()
                {
                    BranchId = deliveryAreaVM.BranchId,
                    RegionId = deliveryAreaVM.RegionId
                };

                await branchDeliveryRepo.AddBranchDeliveryArea(temp);
                return RedirectToAction("GetBranchDeliveringArea", new { Id = branch.Id });
            }
            ViewBag.Cities = await cityRepo.GetAllAsync();
            ViewBag.BranchId = deliveryAreaVM.BranchId;
            return View(deliveryAreaVM);
        }


        public async Task<IActionResult> GetBranchDeliveringArea(int Id)  //BranchId
        {
            var spec = new BranchwithResturantSpecification(Id);
            var branch = await branchRepo.GetByIdWithSpec(spec);
            if (branch == null) return BadRequest();

            ViewBag.BranchId = Id;
            ViewBag.ResturantId = branch.Resturant_ID;
            return View(branch.DeliveryAreas.ToList());
        }



        //Delete Delivery Area from Branch
        public async Task<IActionResult> DeleteBranchDeliveringArea(Available_Delivery_Area delivery_Area)  //BranchId
        {
            var tempArea = (await branchDeliveryRepo.GetBranchDeliveryArea(delivery_Area.BranchId, delivery_Area.RegionId));
            if (tempArea is null) return NotFound();


            await branchDeliveryRepo.DeleteBranchDeliveryArea(tempArea);
            return RedirectToAction(nameof(GetBranchDeliveringArea), new { Id = delivery_Area.BranchId });
        }

    }
}
