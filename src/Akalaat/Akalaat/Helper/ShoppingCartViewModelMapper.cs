using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Akalaat.Helper
{
    public static class ShoppingCartViewModelMapper
    {
        public static ShoppingCartItemVM MapToViewModel(ShoppingCart shoppingCart)
        {
            //var itemSelectList = shoppingCart.Items.Select(item => new SelectListItem
            //{
            //    Value = item.Id.ToString(),
            //    Text = item.Name
            //}).ToList();
            return new ShoppingCartItemVM
            {
               // Items= itemSelectList,
             //  Items = shoppingCart.Items,
                Id=shoppingCart.Id,
              //  Quantity = shoppingCart.Quantity,
             //   Customer_ID = shoppingCart.Customer_ID,
                //Customer = shoppingCart.Customer
            };
        }
    }
}
