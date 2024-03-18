using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Akalaat.Controllers
{
    public class LocationController : Controller
    {
        private readonly IGenericRepository<City> cityRepo;

        public LocationController(IGenericRepository<City>cityRepo)
        {
            this.cityRepo = cityRepo;
        }

        public async Task<IActionResult> SetLocation()
        {
            ViewBag.Cities = await cityRepo.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetLocation([FromBody]LocationVM locationVM)
        {

            HttpContext.Session.SetString("CityId", locationVM.cityId.ToString());
            HttpContext.Session.SetString("DistrictId", locationVM.districtId.ToString());
            HttpContext.Session.SetString("RegionId", locationVM.regionId.ToString());

            return RedirectToAction("Index","Home");
        }

        public IActionResult GetLocation()
        {
            var cityResult = int.TryParse(HttpContext.Session.GetString("CityId"), out int cityId);
            var districtResult = int.TryParse(HttpContext.Session.GetString("DistrictId"), out int districtId);
            var regionResult = int.TryParse(HttpContext.Session.GetString("RegionId"), out int regionId);

            return Json(new { CityId = cityId, DistrictId = districtId, RegionId = regionId });
        }
    }
}
