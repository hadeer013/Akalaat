using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Akalaat.Controllers
{
    public class LocationController : Controller
    {
        private readonly ISession session;

        public LocationController(ISession session)
        {
            this.session = session;
        }

        public IActionResult SetLocation(LocationVM locationVM)
        {
            session.SetString("CityId", locationVM.CityId.ToString());
            session.SetString("DistrictId", locationVM.DistrictId.ToString());
            session.SetString("RegionId", locationVM.RegionId.ToString());

            return RedirectToAction("Index");
        }

        public IActionResult GetLocation()
        {
            var cityId = int.Parse(session.GetString("CityId"));
            var districtId = int.Parse(session.GetString("DistrictId"));
            var regionId = int.Parse(session.GetString("RegionId"));


            return Json(new { CityId = cityId, DistrictId = districtId, RegionId = regionId });
        }
    }
}
