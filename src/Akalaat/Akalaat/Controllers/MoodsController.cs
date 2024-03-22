using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Akalaat.Controllers
{
    public class MoodsController : Controller
    {
        private readonly IGenericRepository<Mood> MoodRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepository<Resturant> RestaurantRepo;
        public MoodsController(IGenericRepository<Mood> MoodRepo, UserManager<ApplicationUser> userManager, IGenericRepository<Resturant> RestaurantRepo)
        {
            this.MoodRepo = MoodRepo;
            this.userManager = userManager;
            this.RestaurantRepo = RestaurantRepo;
        }
        // GET: MoodsController
        public async Task<IActionResult> Index()
        {
            var moods = await MoodRepo.GetAllAsync(includeProperties: "resturant_Moods");
            return View(moods);
        }

        public async Task<IActionResult> Details(int id)
        {
            var mood = await MoodRepo.GetByIdAsync(id);
            if (mood == null)
            {
                return NotFound();
            }
            return View(mood);
        }
        public async Task<IActionResult> Create()
        {

            var restaurants = await RestaurantRepo.GetAllAsync();
            var restaurantsList = restaurants.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });
            var moodVM = new MoodVM { RestaurantsList = restaurantsList };
            return View(moodVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoodVM moodVM)
        {
            Mood moodEntity = new Mood
            {
                Name = moodVM.Name
            };

            var restaurant = await RestaurantRepo.GetByIdAsync(moodVM.RestaurantId);
            if (restaurant != null)
            {
                moodEntity.resturant_Moods.Add(restaurant);
            }

            await MoodRepo.Add(moodEntity);

            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Create()
        //{
        //    var restaurants = await RestaurantRepo.GetAllAsync();
        //    var restaurantsList = restaurants.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });
        //    var moods = await MoodRepo.GetAllAsync();
        //    var moodList = new SelectList(moods, "Id", "Name");

        //    var moodVM = new MoodVM { RestaurantsList = restaurantsList, MoodList = moodList };
        //    return View(moodVM);
        //}
        //public async Task<IActionResult> Create()
        //{
        //    var restaurants = await RestaurantRepo.GetAllAsync();
        //    var restaurantsList = restaurants.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });

        //    var moodVM = new MoodVM { RestaurantsList = restaurantsList };
        //    return View(moodVM);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(MoodVM moodVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Mood moodEntity = new Mood
        //        {
        //            Name = moodVM.Name,

        //        };


        //        await MoodRepo.Add(moodEntity);

        //        return RedirectToAction(nameof(Index));
        //    }

        //    // If validation fails, repopulate view model and return view
        //    var restaurants = await RestaurantRepo.GetAllAsync();
        //    var restaurantsList = restaurants.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });

        //    moodVM.RestaurantsList = restaurantsList;

        //    return View(moodVM);
        //}



        public async Task<IActionResult> Delete(int id)
        {
            var Mood = await MoodRepo.GetByIdAsync(id);
            if (Mood == null)
            {
                return NotFound();
            }
            return View(Mood);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await MoodRepo.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            await MoodRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var mood = await MoodRepo.GetByIdAsync(id);
            if (mood == null)
            {
                return NotFound();
            }

            var restaurants = await RestaurantRepo.GetAllAsync();
            var restaurantsList = restaurants.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name });

            MoodVM moodVM = new MoodVM
            {
                Name = mood.Name,

                RestaurantsList = restaurantsList
            };

            return View(moodVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MoodVM moodVM)
        {
            var mood = await MoodRepo.GetByIdAsync(id);
            if (mood == null)
            {
                return NotFound();
            }

            mood.Name = moodVM.Name;
            //All CRUDs
            //assign mood to restornat 
            //restorant id, mood id
            //remove mood from restorant mood id, restorant id 
            //get all moods in a restorant 
            //takes restorant id and show all moods assotated with restorant 
            var restaurant = await RestaurantRepo.GetByIdAsync(moodVM.RestaurantId);
            if (restaurant != null)
            {
                mood.resturant_Moods.Clear();
                mood.resturant_Moods.Add(restaurant);
            }
            await MoodRepo.Update(mood);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index2(int id)
        {
            var restaurant = await RestaurantRepo.GetByIdIncludingAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }
        //public async Task<IActionResult> get1(int id)
        //{
        //    var restaurant = await RestaurantRepo.GetByIdAsync(id);
        //    if (restaurant == null)
        //    {
        //        return NotFound();
        //    }

        //    // Create a view model to hold restaurant and mood data
        //    RestaurantDetailsVM viewModel = new RestaurantDetailsVM
        //    {
        //        Restaurant = restaurant,
        //        Moods = restaurant.Moods.ToList() // Convert to a list for easier display
        //    };

        //    return View(viewModel);
        //}
        //public async Task<IActionResult> GetMoods(int id)
        //{
        //    // Retrieve all moods (since GetAllAsync doesn't support filtering)
        //    var moods = await MoodRepo.GetAllAsync();

        //    // Filter moods in-memory based on restaurant ID
        //    var filteredMoods = moods.Where(m => m.Id == id).ToList();

        //    return Json(filteredMoods); // Return filtered moods as JSON
        //}
        public async Task<IActionResult> DisplayRestaurantWithMoods(int id)
        {
            
            // Retrieve the restaurant by its ID
            var restaurant = await RestaurantRepo.GetByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

 
            var moods = await MoodRepo.GetAllAsync(
                filters: new List<Expression<Func<Mood, bool>>>
                {
            mood => mood.resturant_Moods.Any(r => r.Id == id)
                },
                includeProperties: "resturant_Moods"
            );
            restaurant.Moods = moods.ToList();

            return View(restaurant);
        }

        public async Task<IActionResult> Assign()
        {
            var moods = await MoodRepo.GetAllAsync();

            var restaurants = await RestaurantRepo.GetAllAsync();

            ViewBag.MoodList = new SelectList(moods, "Id", "Name");
            ViewBag.RestaurantList = new SelectList(restaurants, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int moodId, int restaurantId)
        {
            var mood = await MoodRepo.GetByIdAsync(moodId);
            if (mood == null)
            {
                return NotFound();
            }

            var restaurant = await RestaurantRepo.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            mood.resturant_Moods.Add(restaurant);
            await MoodRepo.Update(mood);

            return RedirectToAction(nameof(Index));
        }
    }
}

