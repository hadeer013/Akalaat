using Akalaat.BLL.Interfaces;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
    public class DeliveryAreaController : Controller
    {
        private readonly IBranchDeliveryRepository branchDeliveryRepo;

        public DeliveryAreaController(IBranchDeliveryRepository branchDeliveryRepo)
        {
            this.branchDeliveryRepo = branchDeliveryRepo;
        }

        public async Task<IActionResult> GetAvailableDistricts(int BranchId,int CityId)
        {
            var allDistricts=await branchDeliveryRepo.GetAllDistrictsAvailableAsDeliveryAreas(BranchId, CityId);
            var Result = new List<AvailableDistrictVM>();
            foreach (var item in allDistricts)
            {
                Result.Add(new AvailableDistrictVM { Id = item.Id, Name = item.Name });
            }
            return Json(Result);
        }

        public async Task<IActionResult> GetAvailableRegions(int BranchId,int DistrictId) 
        {
            var allRegions=await branchDeliveryRepo.GetAllRegionAvailableAsDeliveryAreas(BranchId, DistrictId);
            return Json(allRegions);
        }
    }
}
