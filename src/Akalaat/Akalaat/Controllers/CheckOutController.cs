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
        public CheckOutController(IGenericRepository<Item> itemRepository, UserManager<ApplicationUser> userManager, IGenericRepository<Menu_Item_Size> menuitemsizeRepository)
        {
            _itemRepository = itemRepository;
            this.userManager = userManager;
            this.menuitemsizeRepository = menuitemsizeRepository;
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
                SuccessUrl = domain + "CheckOut/SuccessfulPayment",
                CancelUrl = domain + "CheckOut/FailedPayment",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email
            };

            var items = await _itemRepository.GetAllAsync();
            var sizes = await menuitemsizeRepository.GetAllAsync();

            foreach (var size in sizes)
            {

                var item = items.FirstOrDefault(i => i.Id == size.Item_ID);
                if (item != null)
                {
                    var sessionListItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)size.Price * 100,
                            Currency = "EGP",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Name,
                            }
                        },
                        Quantity = 1
                    };
                    options.LineItems.Add(sessionListItem);
                }
            }
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult SuccessfulPayment()
        {
            return View();
        }
        public IActionResult FailedPayment()
        {
            return View();
        }
    }
}
