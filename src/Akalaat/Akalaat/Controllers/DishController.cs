using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
    public class DishController : Controller
    {

        private readonly IGenericRepository<Dish> DishRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public DishController(IGenericRepository<Dish> DishRepo, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            this.DishRepo = DishRepo;
            this.userManager = userManager;
            hostingEnvironment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var dishes = await DishRepo.GetAllAsync();
            return View(dishes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dish = await DishRepo.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        public IActionResult AddDish()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDish(DishVM dishVM, IFormFile dishImage)
        {
            //   if (ModelState.IsValid && dishImage != null)
            // {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(dishImage.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dishImage.CopyToAsync(fileStream);
            }

            var dishEntity = new Dish
            {
                Name = dishVM.Name,
                Dish_image = uniqueFileName
            };

            await DishRepo.Add(dishEntity);

            return RedirectToAction(nameof(Index));
            //}

            //   return View(dishVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dish = await DishRepo.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await DishRepo.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            await DishRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dish = await DishRepo.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            var dishVM = new DishVM
            {
                Name = dish.Name,
                Resturant_Id = dish.Id
            };

            return View(dishVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DishVM dishVM, IFormFile dishImage)
        {
            if (id != dishVM.Resturant_Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            var dish = await DishRepo.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            // Check if a new image is provided
            if (dishImage != null && dishImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(dishImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dishImage.CopyToAsync(fileStream);
                }

                dish.Dish_image = uniqueFileName;
            }
            else
            {
                // If no new image is provided, keep the old image
                dish.Dish_image = dish.Dish_image;
            }

            // Update the dish name
            dish.Name = dishVM.Name;

            await DishRepo.Update(dish);
            return RedirectToAction(nameof(Index));
        }

        //    return View(dishVM);
        //}


        //  return View(dishVM);
        //}
    }
}
