using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.OrderSpec;
using Akalaat.DAL.Models;
using Akalaat.Helper;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akalaat.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Item> _itemRepository;

        public OrderController(IGenericRepository<Order> orderRepository, IGenericRepository<Item> itemRepository)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new OrderWithCustomerAndItemSpecification();
           
            var orders = await _orderRepository.GetAllWithSpec(spec);

            var orderVMs = orders.Select(order => OrderViewModelMapper.MapToViewModel(order)).ToList();

            return View(orderVMs);
        }

        public async Task<IActionResult> Create()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orderViewModel = new OrderVM
            {
                Customer_ID = customerId
            };
            ViewBag.AllItems =await _itemRepository.GetAllAsync();

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderVM orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    // Map properties from the view model
                    DateTime = orderViewModel.DateTime,
                    Arrival_Time = orderViewModel.ArrivalTime,
                    Total_Price = orderViewModel.TotalPrice,
                    Total_Discount = orderViewModel.TotalDiscount,
                    Customer_ID = orderViewModel.Customer_ID
                };

                // Save the order
                await _orderRepository.Add(order);

                // Redirect to the Index action after successful creation
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Invalid Item");
            return View(orderViewModel);
        }
    }
}
