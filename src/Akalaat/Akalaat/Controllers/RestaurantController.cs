using System.Security.AccessControl;
using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Repositories;
using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers;

public class VendorController:Controller
{
    private readonly IGenericRepository<Vendor> _vendorRepository;
    private readonly IGenericRepository<Resturant> _restaurantRepository;

    private readonly IWebHostEnvironment _webHostEnvironment;



    public VendorController(IGenericRepository<Vendor>  vendorRepository,IGenericRepository<Resturant>  restaurantRepository,
        IWebHostEnvironment webHostEnvironment)
    {
        _vendorRepository = vendorRepository;
        _restaurantRepository = restaurantRepository;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var allRestaurants = await _restaurantRepository.GetAllAsync();
        var firstRestaurant = allRestaurants.FirstOrDefault();
        return View(firstRestaurant);
    }
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