using System.Security.AccessControl;
using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.BLL.Specifications.EntitySpecs.ResturantSpec;
using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers;

public class RestaurantController:Controller
{
    private readonly IGenericRepository<Vendor> _vendorRepository;
    private readonly IGenericRepository<Resturant> _restaurantRepository;

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IGenericRepository<City> cityRepo;

    public RestaurantController(IGenericRepository<Vendor>  vendorRepository,IGenericRepository<Resturant>  restaurantRepository,
        IWebHostEnvironment webHostEnvironment,IGenericRepository<City> cityRepo)
    {
        _vendorRepository = vendorRepository;
        _restaurantRepository = restaurantRepository;
        _webHostEnvironment = webHostEnvironment;
        this.cityRepo = cityRepo;
    }


    [AllowAnonymous]
    public async Task<IActionResult> Home()
    {
        ViewBag.Cities= await cityRepo.GetAllAsync();
        return View();
    }

    //[HttpPost]
    public async Task<IActionResult> Index(ResturantParams resturantPrams)
    {
        var spec = new ResturantWithDishSpecification(resturantPrams.sort, resturantPrams.dishId, resturantPrams.RegionId, resturantPrams.RestaurantName);
        var AllResturantWithSpec = await _restaurantRepository.GetAllWithSpec(spec);

        return View(AllResturantWithSpec);

	}

    public async Task<IActionResult> ResturantDetails(/*int Id*/)
    {
        //var restaurant = await _restaurantRepository.GetByIdAsync(Id);
        //return View(restaurant);
        var allrestaurants = await _restaurantRepository.GetAllAsync();
        var firstrestaurant = allrestaurants.FirstOrDefault();
        return View(firstrestaurant);
    }

    //public async task<iactionresult> index()
    //{
    //    var allrestaurants = await _restaurantrepository.getallasync();
    //    var firstrestaurant = allrestaurants.firstordefault();
    //    return view(firstrestaurant);
    //}

    [HttpPost]
    // [Route("/Edit/CoverImage/{coverImage}")]
    public async Task<IActionResult> EditCoverImage(int id,IFormFile Cover_URL)
    {
        if (Cover_URL != null && Cover_URL.Length > 0)
        {
            try
            {
                //get restaurant info
                var restaurant = await _restaurantRepository.GetByIdAsync(id);
                
                // Generate unique file name
                var fileName = "CoverImage" + Path.GetExtension(Cover_URL.FileName);
                    
                // Save file to wwwroot/images folder
                var DirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Vendor",restaurant?.Id.ToString() ?? string.Empty);
                var imagePath = Path.Combine(DirectoryPath,fileName);

               //create directories if not exist
               if (!Directory.Exists(DirectoryPath))
               {
                    Directory.CreateDirectory(DirectoryPath);
                  
               }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Cover_URL.CopyToAsync(stream);
                }

                // Update database with file path
               
                if (restaurant != null)
                {
                    restaurant.Cover_URL = $"/Vendor/{restaurant.Id}/" + fileName; 
                    await _restaurantRepository.Update(restaurant);
                }

                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error uploading cover image: " + ex.Message);
            }
        }
        return RedirectToAction("Index"); // Redirect to the page where you want to display the uploaded image
    }
   

    // Action for uploading logo image (similar to the UploadCoverImage action)
    public async Task<IActionResult> EditLogoImage(int id,IFormFile Logo_Url)
    {
        if (Logo_Url != null && Logo_Url.Length > 0)
        {
            try
            {
                //get restaurant info
                var restaurant = await _restaurantRepository.GetByIdAsync(id);
                
                // Generate unique file name
                var fileName = "LogoImage" + Path.GetExtension(Logo_Url.FileName);
                    
                // Save file to wwwroot/images folder
                var DirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Vendor",restaurant?.Id.ToString() ?? string.Empty);
                var imagePath = Path.Combine(DirectoryPath,fileName);

                //create directories if not exist
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                  
                }
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Logo_Url.CopyToAsync(stream);
                }

                // Update database with file path
               
                if (restaurant != null)
                {
                    restaurant.Logo_URL = $"/Vendor/{restaurant.Id}/" + fileName; 
                    await _restaurantRepository.Update(restaurant);
                }

                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error uploading cover image: " + ex.Message);
            }
        }
        return RedirectToAction("Index"); // Redirect to the page where you want to display the uploaded image
    }
    public async Task<IActionResult> EditResturantTitle(int id,string Name)
    {
        if (ModelState.IsValid)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
           if(restaurant!=null)
                restaurant.Name = Name;
           await _restaurantRepository.Update(restaurant);
        }
        
           
        return RedirectToAction("Index"); 
        
    }
   
}