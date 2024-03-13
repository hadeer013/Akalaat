using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.AddressBookSpec;
using Akalaat.BLL.Specifications.EntitySpecs.BranchSpec;
using Akalaat.DAL.Models;
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

        public BranchController(IGenericRepository<Resturant> resturantRepo,IGenericRepository<Branch> branchRepo,
            UserManager<ApplicationUser> userManager,IGenericRepository<Address_Book> addressRepo)
        {
            this.resturantRepo = resturantRepo;
            this.branchRepo = branchRepo;
            this.userManager = userManager;
            this.addressRepo = addressRepo;
        }
        //this needs login or not 
        //[AllowAnonymous]
        //public async IActionResult Index(int Id,string Name="") //ResturantId
        //{
        //    var branchSpec = new BranchwithResturantSpecification(Id, Name);
        //    var branches = await branchRepo.GetAllWithSpec(branchSpec);


        //    bool deliverToYou = false;
        //    var ListVM = new List<viewBranchVM>();

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        //        var spec = new AddresswithRegionSpec(user.Id);
        //        var userAddress=await addressRepo.GetByIdWithSpec(spec);
        //        if (userAddress != null)
        //        {
        //            foreach(var item in branches)
        //    {
        //                var branchVM = new viewBranchVM()
        //                {
        //                    AddressDetails = item.AddressDetails,
        //                    CloseHour = item.Close_Hour,
        //                    OpenHour = item.Open_Hour,
        //                    Id = item.Id,
        //                    RegionName = item.Region.Name,
        //                    DeliverToYou = deliverToYou,
        //                    IsAuthenticated = true,
        //                    DeliveringAreas = item?.DeliveryAreas?.Select(d => d.Name).ToList()
        //                };
        //                ListVM.Add(branchVM);

        //            }
        //        }
        //    }
        //    else
        //    {

        //    }


           

        //    f


        //    return View(ListVM);
        //}

        
        //public async Task<IActionResult> AddBranch(int ResturantId)
        //{
        //    var Resurant= await resturantRepo.GetByIdAsync(ResturantId);
        //    if (Resurant == null) return BadRequest();

        //}

        //
    }
}
