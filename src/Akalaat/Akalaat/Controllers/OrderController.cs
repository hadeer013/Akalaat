using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.CustomerSpec;
using Akalaat.BLL.Specifications.EntitySpecs.OrderSpec;
using Akalaat.DAL.Models;
using Akalaat.Helper;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Akalaat.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Item> _itemRepository;
        private readonly IGenericRepository<ShoppingCart> ShoppingCartRepository;
        private readonly IGenericRepository<ShoppingCartItem> ShoppingCartItemRepository;
        private readonly IGenericRepository<Customer> CustomerRepository;
        public OrderController(IGenericRepository<Order> orderRepository, IGenericRepository<Item> itemRepository, IGenericRepository<ShoppingCart> ShoppingCartRepository, IGenericRepository<ShoppingCartItem> ShoppingCartItemRepository, IGenericRepository<Customer> CustomerRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            this.ShoppingCartRepository = ShoppingCartRepository;
            this.ShoppingCartItemRepository = ShoppingCartItemRepository;
            this.CustomerRepository = CustomerRepository;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new OrderWithCustomerAndItemSpecification();
        
            var orders = await _orderRepository.GetAllWithSpec(spec);
        
            var orderVMs = orders.Select(order => OrderViewModelMapper.MapToViewModel(order)).ToList();
        
            return View(orderVMs);
        }
        public async Task<IActionResult> CustomerIndex()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _orderRepository.GetAllAsync([(or =>or.Customer_ID==customerId)],includeProperties:"Customer");

            var orderVMs = orders.Select(order => OrderViewModelMapper.MapToViewModel(order)).ToList();

            return View(orderVMs);
        }

        public async Task<IActionResult> Create()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var items = await _itemRepository.GetAllAsync();

            var itemSelectList = items.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });

            var orderViewModel = new OrderVM
            {
                Customer_ID = customerId,
                DateTime = DateTime.Now,
                ArrivalTime = DateTime.Now.AddHours(1),
                Items = itemSelectList.ToList()
            };

            return View(orderViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Create(float? TotalPrice /*[FromBody]CartVM cartVM*/)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerWithShoppingCartSpecification spec = new CustomerWithShoppingCartSpecification(customerId);

            var CurrentCustomer = await CustomerRepository.GetByIdWithSpec(spec);
            var Shopping_ID = CurrentCustomer.ShoppingCart_ID;
            ShoppingCart shoppingCart = await ShoppingCartRepository.GetByIdAsync(Shopping_ID);
            var shoppingCartItem = await ShoppingCartItemRepository.GetAllAsync([item => item.ShoppingCartId == Shopping_ID], includeProperties: "Item");
            List<Item> _items = new List<Item>();

            if (shoppingCartItem.Count != 0)
            {
                foreach (var item in shoppingCartItem)
                {
                    _items.Add(item.Item);
                }
            }
            if (ModelState.IsValid)
            {

                var order = new Order
                {

                    DateTime = DateTime.Now,
                    Total_Price = (int)TotalPrice,
                    Items = _items,
                    Customer_ID = customerId,

                };

                await _orderRepository.Add(order);
                if (shoppingCartItem.Count != 0)
                {
                    for (var i = 0; i < shoppingCartItem.Count; i++)
                    {
                        var item = shoppingCartItem[i];
                        await ShoppingCartItemRepository.Delete(item.ItemId, item.ShoppingCartId);
                    }
                }
                if (shoppingCart != null)
                {
                    shoppingCart.TotalPrice = null;
                    await ShoppingCartRepository.Update(shoppingCart);
                }
                return RedirectToAction("Index");
            }
            var items = await _itemRepository.GetAllAsync();

            var itemSelectList = items.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name
            });
            // orderViewModel.Items = itemSelectList.ToList();

            ModelState.AddModelError("", "Invalid Item");
            return View();
        }
        public async Task<IActionResult> Details(int Id)
        {
            // Expression<Func<Order, object>>[] includes = { o => o.Customer, o => o.Items };
            var spec = new OrderWithCustomerAndItemSpecification(Id);

            var order = await _orderRepository.GetByIdWithSpec(spec);

            if (order == null)
            {
                return View();
            }

            var orderViewModel = OrderViewModelMapper.MapToViewModel(order);

            return View(orderViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var spec = new OrderWithCustomerAndItemSpecification(id);

            var order = await _orderRepository.GetByIdWithSpec(spec);
            var items = await _itemRepository.GetAllAsync();

            var itemSelectList = items.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
                Selected = order.Items.Any(i => i.Id == item.Id)
            });

            if (order == null)
            {
                return View();
            }

            var orderViewModel = OrderViewModelMapper.MapToViewModel(order);

            orderViewModel.SelectedItemS = items.Select(I => I.Id).ToList();

            return View(orderViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderVM orderViewModel)
        {
            if (id != orderViewModel.Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                var spec = new OrderWithCustomerAndItemSpecification(id);

                var order = await _orderRepository.GetByIdWithSpec(spec);
                if (order == null)
                {
                    return View();
                }

                ICollection<Item> _Items = new HashSet<Item>();

                foreach (var Id in orderViewModel.SelectedItemS)
                {
                    var item = await _itemRepository.GetByIdAsync(Id);
                    _Items.Add(item);
                }
                order.DateTime = orderViewModel.DateTime;
                order.Arrival_Time = orderViewModel.ArrivalTime;
                order.Total_Price = orderViewModel.TotalPrice;
                order.Total_Discount = orderViewModel.TotalDiscount;
                order.Customer_ID = orderViewModel.Customer_ID;
                order.Items = _Items.ToList();
                await _orderRepository.Update(order);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Invalid Item");
            ViewBag.AllItems = await _itemRepository.GetAllAsync();
            return View(orderViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = OrderViewModelMapper.MapToViewModel(order);
            return View(orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
