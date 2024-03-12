using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.BLL.Specifications.EntitySpecs.RegionSpec;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Akalaat.Controllers
{
	public class DistrictController : Controller
	{
		private readonly IGenericRepository<City> cityRepo;
		private readonly IGenericRepository<District> genericRepo;
        private readonly IGenericRepository<Region> regionRepo;
        private readonly IDistrictRepository districtRepo;

        public DistrictController(IGenericRepository<City> cityRepo,IGenericRepository<District> genericRepo,
			IGenericRepository<Region> regionRepo, IDistrictRepository districtRepo)
		{
			this.cityRepo = cityRepo;
			this.genericRepo = genericRepo;
            this.regionRepo = regionRepo;
            this.districtRepo = districtRepo;
        }
		

		public async Task<IActionResult> Details(int id,string RegionName="")
		{
			var district = await genericRepo.GetByIdAsync(id);
			if (district == null) return BadRequest();

			var districtSpes = new DistrictWithRegionSpecification(id, RegionName);
			var AllRegions = await regionRepo.GetAllWithSpec(districtSpes);

            if (AllRegions == null) return BadRequest();

			
			ViewBag.Regions = AllRegions;
			return View(district);
        }


        public async Task<IActionResult> Create(int id) //cityId i want to add this district to
		{
			var city=await cityRepo.GetByIdAsync(id);
			if (city == null) return BadRequest();

			var DistrictVM= new DistrictVM() { City_ID=id, City_Name=city.Name};

			return View(DistrictVM);
		}


		[HttpPost]
		public async Task<IActionResult> Create(DistrictVM districtVM)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var city = await cityRepo.GetByIdAsync(districtVM.City_ID);
					if (city == null) return BadRequest();

					var district=new District() { City_ID=city.Id, Name=districtVM.Name};
					var added = await genericRepo.Add(district);

					return RedirectToAction("Details","City", new { id = city.Id });
				}
				catch
				{
					return View("Error", new ErrorViewModel() { Message = "Could not add this district", RequestId = "1001" });
				}
			}
			return View(districtVM);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var district = await genericRepo.GetByIdAsync(id);
			if (district == null) return BadRequest();


			return View(new EditDistrictVM() { ID=id, Name=district.Name, CityId=district.City_ID});
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditDistrictVM editDistrictVM)
		{
			if(id!=editDistrictVM.ID) return BadRequest();
			if (ModelState.IsValid)
			{
				var district=await genericRepo.GetByIdAsync(id);
				if(district == null) return BadRequest();

				district.Name = editDistrictVM.Name;
				await genericRepo.Update(district);
				return RedirectToAction("Details", "City", new { id = district.City_ID });
			}
			return View(editDistrictVM);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var district=await genericRepo.GetByIdAsync(id);
			if(district == null) return BadRequest();

			await genericRepo.Delete(id);
			return RedirectToAction("Details", "City", new { id = district.City_ID });
		}



        ///For AddressBook Controller
        [HttpGet]
        public async Task<IActionResult> GetDistrictsByCityId(int cityId)
        {
            var districts = await districtRepo.GetAllDistrictsByCityId(cityId);
            return Json(districts);
        }
    }
}
