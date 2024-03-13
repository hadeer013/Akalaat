using Akalaat.DAL.Models;
using Akalaat.ViewModels;

namespace Akalaat.Helper
{
    public class OrderViewModelMapper
    {
        public static OrderVM MapToViewModel(Order order)
        {
            return new OrderVM
            {
                Id=order.Id,
                DateTime = order.DateTime,
                ArrivalTime = order.Arrival_Time,
                TotalPrice = order.Total_Price,
                TotalDiscount = order.Total_Discount,
                Customer_ID = order.Customer_ID,
                Customer = order.Customer,
                OrderItems = order.OrderItems,
            };
        }
    }

}
