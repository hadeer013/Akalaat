using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.OrderSpec;
using Akalaat.DAL.Models;
using Akalaat.Helper;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGenericRepository<Order> _orderRepository;

        public OrderController(IGenericRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new OrderWithCustomerAndItemSpecification();
            var orders = await _orderRepository.GetAllWithSpec(spec);

            var orderVMs = orders.Select(order => OrderViewModelMapper.MapToViewModel(order)).ToList();

            return View(orderVMs);
        }

       
    }
}
