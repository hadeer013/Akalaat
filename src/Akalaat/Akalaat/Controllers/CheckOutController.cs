using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Akalaat.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IGenericRepository<Item> _itemRepository;
        private readonly IGenericRepository<Menu_Item_Size> menuitemsizeRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGenericRepository<Order> OrderRepository;
        private readonly IGenericRepository<ShoppingCartItem> ShoppingCartRepository;
        public CheckOutController(IGenericRepository<Item> itemRepository, UserManager<ApplicationUser> userManager, IGenericRepository<Menu_Item_Size> menuitemsizeRepository, IGenericRepository<Order> OrderRepository, IGenericRepository<ShoppingCartItem> ShoppingCartRepository)
        {
            _itemRepository = itemRepository;
            this.userManager = userManager;
            this.menuitemsizeRepository = menuitemsizeRepository;
            this.OrderRepository = OrderRepository;
            this.ShoppingCartRepository = ShoppingCartRepository;

        }
        public async Task<IActionResult> Checkout()
        {
            var domain = "http://localhost:5208/";
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "Order/Create",
                CancelUrl = domain + "CheckOut/FailedPayment",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email
            };

            // Retrieve items, sizes, and orders from the database
            var items = await _itemRepository.GetAllAsync();
            var orders = await OrderRepository.GetAllAsync();
            var shoppingCartItems = await ShoppingCartRepository.GetAllAsync(); // Make sure you have a repository for ShoppingCartItems

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var item = items.FirstOrDefault(i => i.Id == shoppingCartItem.ItemId);
                if (item != null)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)item.Price * 100,
                            Currency = "EGP",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name,
                            }
                        },
                        Quantity = shoppingCartItem.Quantity // Assuming Quantity is available in ShoppingCartItems table
                    };
                    options.LineItems.Add(sessionListItem);
                }
            }

            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect to the payment URL
            return Redirect(session.Url);
        }
        public async Task<IActionResult> OrderSummary()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var shoppingCartItems = await ShoppingCartRepository.GetAllAsync();
            var items = await _itemRepository.GetAllAsync();
            decimal totalPrice = 0;
            List<string> purchasedItemNames = new List<string>();
            Dictionary<string, decimal> itemPrices = new Dictionary<string, decimal>();

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var item = items.FirstOrDefault(i => i.Id == shoppingCartItem.ItemId);
                if (item != null)
                {
                    decimal itemTotalPrice = (decimal)item.Price * (decimal)shoppingCartItem.Quantity;
                    totalPrice += itemTotalPrice;
                    purchasedItemNames.Add(item.Name);
                    itemPrices.Add(item.Name, itemTotalPrice); // Add item price to dictionary
                }
            }

            ViewData["PurchasedItemNames"] = purchasedItemNames;
            ViewData["ItemPrices"] = itemPrices; // Pass item prices to the view

            return View();
        }

        public IActionResult SuccessfulPayment()
        {

            return View();
        }

        [HttpPost]
        //public async Task<IActionResult> CreateOrder(OrderVM orderVM)
        //{
        //    // Assuming you have the necessary logic to create an order
        //    // You can retrieve the necessary information from the orderVM parameter

        //    // Once the order is created, you can redirect the user to a confirmation page or any other appropriate action
        //    return RedirectToAction("OrderConfirmation");
        //}
        public IActionResult FailedPayment()
        {
            return View();
        }
    }
}
