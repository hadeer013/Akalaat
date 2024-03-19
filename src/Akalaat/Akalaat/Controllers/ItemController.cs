using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace Akalaat.Controllers
{
    public class ItemController : Controller
    {
        private readonly IGenericRepository<Item> _itemRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private IGenericRepository<Menu> _menuRepository;
        private readonly IGenericRepository<Menu_Item_Size> menuitemsizeRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public ItemController(IGenericRepository<Item> itemRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<Menu> menuRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _menuRepository = menuRepository;
            this.userManager = userManager;
            this.menuitemsizeRepository = menuitemsizeRepository;
        }


        //vendor view
        public async Task<IActionResult> Index(int menuId, int categoryId = -1, string searchItem = "")
        {
            // Retrieve restaurant name from the menu ID using the existing repositories or context
            var menu = await _menuRepository.GetByIdIncludingAsync(menuId, m => m.Resturant);
            var restaurantName = menu?.Resturant?.Name;
            var orderBy = (Func<IQueryable<Item>, IOrderedQueryable<Item>>)null;
            orderBy = items => items.OrderByDescending(item => item.Likes);

            // Initialize filters list
            var filters = new List<Expression<Func<Item, bool>>>();

            // Add filter for MenuID
            filters.Add(item => item.MenuID == menuId);

            // Add filter for CategoryID if provided
            if (categoryId != -1)
            {
                filters.Add(item => item.CategoryID == categoryId);
            }


            // Add filter for SearchItem if provided
            if (!string.IsNullOrEmpty(searchItem))
            {
                filters.Add(item => item.Name.Contains(searchItem) || item.Description.Contains(searchItem));
            }

            // Retrieve items based on filters
            var items = await _itemRepository.GetAllAsync(filters, orderBy);

            // Map items to view models
            var itemViewModels = items.Select(MapToViewModel).ToList();

            // Populate ViewBag properties
            ViewBag.MenuID = menuId;
            ViewBag.Categories = new SelectList(
                await _categoryRepository.GetAllAsync([category => category.Menu_ID == menuId]),
                "Id", "Name");
            ViewBag.RestaurantName = restaurantName;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchValue = searchItem;

            // Return view with view models
            return View(itemViewModels);
        }


        public async Task<IActionResult> IndexToCustomer(int menuId, int categoryId = -1, string searchItem = "")
        {
            
            // var items = await _itemRepository.GetAllAsync();
            // var itemViewModels = items.Select(MapToViewModel).ToList();
            // return View(itemViewModels);
            // Retrieve restaurant name from the menu ID using the existing repositories or context
            var menu = await _menuRepository.GetByIdIncludingAsync(menuId, m => m.Resturant);
            var restaurantName = menu?.Resturant?.Name;
            var orderBy = (Func<IQueryable<Item>, IOrderedQueryable<Item>>)null;
            orderBy = items => items.OrderByDescending(item => item.Likes);

            // Initialize filters list
            var filters = new List<Expression<Func<Item, bool>>>();

            // Add filter for MenuID
            filters.Add(item => item.MenuID == menuId);

            // Add filter for CategoryID if provided
            if (categoryId != -1)
            {
                filters.Add(item => item.CategoryID == categoryId);
            }


            // Add filter for SearchItem if provided
            if (!string.IsNullOrEmpty(searchItem))
            {
                filters.Add(item => item.Name.Contains(searchItem) || item.Description.Contains(searchItem));
            }

            // Retrieve items based on filters
            var items = await _itemRepository.GetAllAsync(filters, orderBy);

            // Map items to view models
            var itemViewModels = items.Select(MapToViewModel).ToList();

            // Populate ViewBag properties
            ViewBag.MenuID = menuId;
            ViewBag.Categories = new SelectList(
                await _categoryRepository.GetAllAsync([category => category.Menu_ID == menuId]),
                "Id", "Name");
            ViewBag.RestaurantName = restaurantName;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchValue = searchItem;

            // Return view with view models
            return View(itemViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int menuId)
        {
            ViewBag.Categories = new SelectList(
                await _categoryRepository.GetAllAsync([category => (category.Menu_ID == menuId)]),
                "Id", "Name");
            return View(new ItemViewModel()
            {
                MenuID = menuId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = new Item()
                {
                    Name = itemViewModel.Name,
                    Description = itemViewModel.Description,
                    Image_URL = "",
                    IsOffer = itemViewModel.IsOffer,
                    Discount = itemViewModel.Discount,
                    MenuID = itemViewModel.MenuID,
                    CategoryID = itemViewModel.CategoryID,
                    Price = itemViewModel.Price,
                };
                await _itemRepository.Add(item);
                if (itemViewModel.Image != null && itemViewModel.Image.Length > 0)
                {
                    string imageName = await SaveImageAsync(itemViewModel);
                    item.Image_URL = imageName;
                    await _itemRepository.Update(item);
                }
                return RedirectToAction("Index", "Item", new { menuID = itemViewModel.MenuID });
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
                    var ItemViewModel = MapToViewModel(item);
                    // var ItemViewModel = new ItemViewModel()
                    // {
                    //     Description = item.Description,
                    //     ImageUrl=item.Image_URL,
                    //     IsOffer = item.IsOffer,
                    //     Discount = item.Discount,
                    //     Likes=item.Likes,
                    //     Name=item.Name,
                    //     Price =item.Price
                    // };
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
            var ItemViewModel = MapToViewModel(item);
            ViewBag.Categories = new SelectList(
                await _categoryRepository.GetAllAsync(),
                "ID", "Name", item.CategoryID);
            // var ItemViewModel = new ItemViewModel()
            // {
            //     Description = item.Description,
            //     ImageUrl = item.Image_URL,
            //     IsOffer = item.IsOffer,
            //     Discount = item.Discount,
            //     Likes = item.Likes,
            //     Name = item.Name,
            //     Price = item.Price
            // };

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
                    existingItem.Description = itemViewModel.Description;
                    existingItem.Discount = itemViewModel.Discount ?? 0;
                    existingItem.IsOffer = itemViewModel.IsOffer;
                    existingItem.CategoryID = itemViewModel.CategoryID;

                    existingItem.Price = itemViewModel.Price;
                    if (itemViewModel.Image != null && itemViewModel.Image.Length > 0)
                    {
                        existingItem.Image_URL = await SaveImageAsync(itemViewModel);
                    }
                    await _itemRepository.Update(existingItem);
                    return RedirectToAction("Index", "Item", new { menuID = itemViewModel.MenuID });
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
            var ItemViewModel = MapToViewModel(item);

            //
            // var ItemViewModel = new ItemViewModel()
            // {
            //     Description = item.Description,
            //     ImageUrl = item.Image_URL,
            //     IsOffer = item.IsOffer,
            //     Discount = item.Discount,
            //     Likes = item.Likes,
            //     Name = item.Name,
            //     Price = item.Price
            //     
            // };

            return View(ItemViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            DeleteImageFile(item.Image_URL);
            await _itemRepository.Delete(id);
            return RedirectToAction("Index", "Item", new { menuID = item.MenuID });
        }

        private ItemViewModel MapToViewModel(Item item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Likes = item.Likes ?? 0,
                Description = item.Description,
                ImageUrl = item.Image_URL,
                Discount = item.Discount,
                IsOffer = item.IsOffer,
                CategoryID = item.CategoryID ?? 0,
                Price =item.Price
            };
        }

        private async Task<string> SaveImageAsync(ItemViewModel itemViewModel)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            string imageName = Guid.NewGuid() + Path.GetExtension(itemViewModel.Image.FileName);
            string finalImagePath = Path.Combine(folderPath, imageName);
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
