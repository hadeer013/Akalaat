using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Akalaat.Controllers
{
    public class RegionController : Controller
    {
        private readonly IGenericRepository<District> districtRepo;
        private readonly IGenericRepository<Region> genericRepo;
        private readonly IRegionRepository regionRepo;

        public RegionController(IGenericRepository<District> districtRepo,IGenericRepository<Region> genericRepo,
            IRegionRepository regionRepo)
        {
            this.districtRepo = districtRepo;
            this.genericRepo = genericRepo;
            this.regionRepo = regionRepo;
        }

        public async Task<ActionResult> Create(int id) //DistrictId i want to add this region to
        {
            var district = await districtRepo.GetByIdAsync(id);
            if (district == null) return BadRequest();


            var vm = new AddRegionVM() { DistrictId = district.Id, DistrictName = district.Name };
            return View(vm);
        }


        [HttpPost]
        public async Task<ActionResult> Create(AddRegionVM regionVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var district = await districtRepo.GetByIdAsync(regionVM.DistrictId);
                    if (district == null) return BadRequest();

                    var region = new Region() { Name=regionVM.RegionName, District_ID=regionVM.DistrictId};
                    var added = await genericRepo.Add(region);

                    return RedirectToAction("Details", "District", new { id = district.Id });
                }
                catch
                {
                    return View("Error", new ErrorViewModel() { Message = "Could not add this district", RequestId = "1001" });
                }
            }
            return View(regionVM);
        }


		public async Task<IActionResult> Edit(int id)
		{
			var region = await genericRepo.GetByIdAsync(id);
			if (region == null) return BadRequest();


			return View(new EditRegionVM() { Id = id, RegionName = region.Name, DistrictId=region.District_ID});
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditRegionVM editRegionVM)
		{
			if (id != editRegionVM.Id) return BadRequest();
			if (ModelState.IsValid)
			{
				var region = await genericRepo.GetByIdAsync(id);
				if (region == null) return BadRequest();

				region.Name = editRegionVM.RegionName;
				await genericRepo.Update(region);
				return RedirectToAction("Details", "District", new { id = region.District_ID});
			}
			return View(editRegionVM);
		}

        public async Task<IActionResult> Delete(int id)
        {
			var region = await genericRepo.GetByIdAsync(id);
			if (region == null) return BadRequest();

            try
            {
                await genericRepo.Delete(id);
				return RedirectToAction("Details", "District", new { id = region.District_ID });
			}
            catch
            {
				return View("Error", new ErrorViewModel() { Message = "Could not delete this region", RequestId = "1001" });
			}
		}


        ///For AddressBook Controller
        [HttpGet]
        public async Task< IActionResult> GetRegionsByDestrictId(int districtId)
        {
            var regions = await regionRepo.GetAllRegionsByDistrictId(districtId);

            return Json(regions);
        }
    }
}
