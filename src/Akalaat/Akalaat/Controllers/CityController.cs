using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.CitySpec;
using Akalaat.DAL.Models;
using Akalaat.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
	public class CityController : Controller
	{
		private readonly IGenericRepository<City> genericRepository;

		public CityController(IGenericRepository<City> genericRepository)
		{
			this.genericRepository = genericRepository;
		}

		public async Task<ActionResult> Index()
		{
			return View(await genericRepository.GetAllAsync());
		}


		public async Task<ActionResult> Details(int id)
		{
			var CitySpec = new CityWithDistrictSpecification(id);
			var city = await genericRepository.GetByIdWithSpec(CitySpec);

			if (city != null)
				return View(city);

			return View("Error", new ErrorViewModel() { Message = "No such city found.", RequestId = "1001" });

		}


		public ActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public async Task<ActionResult> Create(AddCityVM addCityVM)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var city = new City() { Name = addCityVM.Name };
					var added = await genericRepository.Add(city);

					return RedirectToAction("Details", new { id = added.Id });
				}
				catch
				{
					return View("Error", new ErrorViewModel() { Message = "Could not add this city", RequestId = "1001" });
				}
			}
			return View(addCityVM);
		}


		public async Task<ActionResult> Edit(int id)
		{
			var city = await genericRepository.GetByIdAsync(id);

			if (city != null)
				return View(new AddCityVM { Name=city.Name,id=city.Id});

			return View("Error", new ErrorViewModel() { Message = "No such city found.", RequestId = "1001" });


		}


		[HttpPost]
		public async Task<ActionResult> Edit(int id,AddCityVM CityVM)
		{
			if (id != CityVM.id) return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var city=await genericRepository.GetByIdAsync(CityVM.id);
					city.Name = CityVM.Name;
					await genericRepository.Update(city);

					return RedirectToAction(nameof(Details), new {id=city.Id});
				}
				catch
				{
					return View(CityVM);
				}
			}
			return View(CityVM);
		}


		public async Task<ActionResult> Delete(int id)
		{
			var city = await genericRepository.GetByIdAsync(id);
			if (city != null)
			{
				await genericRepository.Delete(id);
				return RedirectToAction(nameof(Index));
			}
				
			return BadRequest();
		}

	}
}
