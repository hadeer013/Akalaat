using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.BLL.Specifications.EntitySpecs.DistrictSpec;
using Akalaat.DAL.Models;
using Akalaat.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
	public class DistrictController : Controller
	{
		private readonly IGenericRepository<City> cityRepo;
		private readonly IGenericRepository<District> districtRepo;

		public DistrictController(IGenericRepository<City> cityRepo,IGenericRepository<District> districtRepo)
		{
			this.cityRepo = cityRepo;
			this.districtRepo = districtRepo;
		}

		public async Task<IActionResult> Details(int id)
		{
			var districtSpes = new DistrictWithRegionSpecification(id);
			var district = await districtRepo.GetByIdWithSpec(districtSpes);

            if (district == null) return BadRequest();

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
					var added = await districtRepo.Add(district);

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
			var district = await districtRepo.GetByIdAsync(id);
			if (district == null) return BadRequest();


			return View(new EditDistrictVM() { ID=id, Name=district.Name, CityId=district.City_ID});
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditDistrictVM editDistrictVM)
		{
			if(id!=editDistrictVM.ID) return BadRequest();
			if (ModelState.IsValid)
			{
				var district=await districtRepo.GetByIdAsync(id);
				if(district == null) return BadRequest();

				district.Name = editDistrictVM.Name;
				await districtRepo.Update(district);
				return RedirectToAction("Details", "City", new { id = district.City_ID });
			}
			return View(editDistrictVM);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var district=await districtRepo.GetByIdAsync(id);
			if(district == null) return BadRequest();

			await districtRepo.Delete(id);
			return RedirectToAction("Details", "City", new { id = district.City_ID });
		}
	}
}
