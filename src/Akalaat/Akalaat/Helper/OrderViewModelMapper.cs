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
                DateTime = order.DateTime,
                ArrivalTime = order.Arrival_Time,
                TotalPrice = order.Total_Price,
                TotalDiscount = order.Total_Discount,
                Customer_ID = order.Customer_ID,
                Customer = order.Customer,
              //  Item_ID = order.Item_ID,
              //  Item = order.Item
            };
        }
    }

}
