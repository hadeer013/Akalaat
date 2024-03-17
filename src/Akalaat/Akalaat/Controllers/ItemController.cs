using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Akalaat.Controllers
{
    public class ItemController : Controller
    {
        private readonly IGenericRepository<Item> _itemRepository;
        private readonly IGenericRepository<Menu_Item_Size> menuitemsizeRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public ItemController(IGenericRepository<Item> itemRepository, UserManager<ApplicationUser> userManager, IGenericRepository<Menu_Item_Size> menuitemsizeRepository)
        {
            _itemRepository = itemRepository;
            this.userManager=userManager;
            this.menuitemsizeRepository = menuitemsizeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemRepository.GetAllAsync();
            var itemViewModels = items.Select(MapToViewModel).ToList();
            return View(itemViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = new Item()
                {
                    Name=itemViewModel.Name,
                    Description = itemViewModel.Description,
                    Image_URL = "",
                    IsOffer = itemViewModel.IsOffer,
                    Discount = itemViewModel.Discount
                };
                await _itemRepository.Add(item);
                if (itemViewModel.Image != null && itemViewModel.Image.Length > 0)
                {
                    // Save the image and get the file name
                    string imageName = await SaveImageAsync(itemViewModel);

                    // Update the item's Image_URL property with the file name
                    item.Image_URL = imageName;

                    // Update the item in the repository with the new Image_URL
                    await _itemRepository.Update(item);
                }
                return RedirectToAction(nameof(Index));

            }

            ModelState.AddModelError("", "Invalid Item");
            return View(itemViewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id != null)
                {
                    var item = await _itemRepository.GetByIdAsync(id);
                    var ItemViewModel = new ItemViewModel()
                    {
                        Description = item.Description,
                        ImageUrl=item.Image_URL,
                        IsOffer = item.IsOffer,
                        Discount = item.Discount,
                        Likes=item.Likes,
                        Name=item.Name
                    };
                    return View(ItemViewModel);

                }
                return View();

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        public async Task<IActionResult> Edit(int id)
        {

            var item = await _itemRepository.GetByIdAsync(id);
            var ItemViewModel = new ItemViewModel()
            {
                Description = item.Description,
                ImageUrl = item.Image_URL,
                IsOffer = item.IsOffer,
                Discount = item.Discount,
                Likes = item.Likes,
                Name = item.Name
            };

            return View(ItemViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModel itemViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = await _itemRepository.GetByIdAsync(id);

                    existingItem.Name = itemViewModel.Name;
                    existingItem.Likes = itemViewModel.Likes ?? 0;
                    existingItem.Description = itemViewModel.Description;
                    existingItem.Discount = itemViewModel.Discount ?? 0;
                    existingItem.IsOffer = itemViewModel.IsOffer;
                    if (itemViewModel.Image != null && itemViewModel.Image.Length > 0)
                    {
                        // Add logic to save the uploaded image and update the existingItem.Image_URL property
                        existingItem.Image_URL = await SaveImageAsync(itemViewModel);
                    }
                    await _itemRepository.Update(existingItem);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid Item");
                    return View(itemViewModel);
                }
            }
            return View(itemViewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
           
            var item = await _itemRepository.GetByIdAsync(id);


            var ItemViewModel = new ItemViewModel()
            {
                Description = item.Description,
                ImageUrl = item.Image_URL,
                IsOffer = item.IsOffer,
                Discount = item.Discount,
                Likes = item.Likes,
                Name = item.Name
            };

            return View(ItemViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            DeleteImageFile(item.Image_URL);

            await _itemRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        private ItemViewModel MapToViewModel(Item item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Likes = item.Likes,
                Description = item.Description,
                ImageUrl = item.Image_URL,
                Discount = item.Discount,
                IsOffer = item.IsOffer,
            };
        }
        private async Task<string> SaveImageAsync(ItemViewModel itemViewModel)
        {
            // Get Directory
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

            // Get File Name
            string imageName = Guid.NewGuid() + Path.GetExtension(itemViewModel.Image.FileName);//"fixed"

            // Merge Path with File Name
            string finalImagePath = Path.Combine(folderPath, imageName);

            // Save File As Streams "Data Overtime"
            using (var stream = new FileStream(finalImagePath, FileMode.Create))
            {
                await itemViewModel.Image.CopyToAsync(stream);
            }

            return imageName;
        }
        private void DeleteImageFile(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", imageUrl);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
        }

        //public async Task<IActionResult> Checkout()
        //{
        //    var domain = "http://localhost:5208/";
        //    var user = await userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Login", "Account"); 
        //    }

        //    var options = new SessionCreateOptions
        //    {
        //        SuccessUrl = domain + "Item/SuccessfulPayment",
        //        CancelUrl = domain + "Item/Index",
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        CustomerEmail = user.Email
        //    };

        //    var items = await _itemRepository.GetAllAsync();
        //    var sizes = await menuitemsizeRepository.GetAllAsync();

        //    foreach (var size in sizes)
        //    {
               
        //        var item = items.FirstOrDefault(i => i.Id == size.Item_ID);
        //        if (item != null)
        //        {
        //            var sessionListItem = new SessionLineItemOptions
        //            {
        //                PriceData = new SessionLineItemPriceDataOptions
        //                {
        //                    UnitAmount = (long)size.Price * 100, 
        //                    Currency = "EGP",
        //                    ProductData = new SessionLineItemPriceDataProductDataOptions
        //                    {
        //                        Name = item.Name,
        //                    }
        //                },
        //                Quantity = 1
        //            };
        //            options.LineItems.Add(sessionListItem);
        //        }
        //    }
        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    Response.Headers.Add("Location", session.Url);
        //    return new StatusCodeResult(303);
        //}
        //public IActionResult SuccessfulPayment() 
        //{
        //    return View();
        //}
    }
}
