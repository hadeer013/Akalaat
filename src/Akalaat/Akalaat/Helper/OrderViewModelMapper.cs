using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Akalaat.Helper
{
    public class OrderViewModelMapper
    {
        public static OrderVM MapToViewModel(Order order)
        {
            var itemSelectList = order.Items.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name 
            }).ToList();
            return new OrderVM
            {
                Id=order.Id,
                DateTime = order.DateTime,
                ArrivalTime = order.Arrival_Time,
                TotalPrice = order.Total_Price,
                TotalDiscount = order.Total_Discount,
                Customer_ID = order.Customer_ID,
                Customer = order.Customer,
                Items = itemSelectList
            };
        }
    }

}
