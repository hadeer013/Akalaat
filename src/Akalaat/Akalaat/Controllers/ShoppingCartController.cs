using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications.EntitySpecs.CustomerSpec;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
 
namespace Akalaat.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IGenericRepository<ShoppingCart> ShoppingCartRepository;
        private readonly IGenericRepository<ShoppingCartItem> ShoppingCartItemRepository;

        private readonly IGenericRepository<Item> itemRepository;
        private readonly IGenericRepository<Customer> CustomerRepository;
        private readonly IGenericRepository<Item> _itemRepository;

        public ShoppingCartController(IGenericRepository<ShoppingCart> ShoppingCartRepository, IGenericRepository<Item> itemRepository, IGenericRepository<Customer> CustomerRepository, IGenericRepository<ShoppingCartItem> shoppingCartItemRepository, IGenericRepository<Item> _itemRepository)
        {
            this.ShoppingCartRepository = ShoppingCartRepository;
            this.itemRepository = itemRepository;
            this.CustomerRepository = CustomerRepository;
            this._itemRepository = _itemRepository;
            ShoppingCartItemRepository = shoppingCartItemRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Item = await itemRepository.GetByIdAsync(Id);
            CustomerWithShoppingCartSpecification spec = new CustomerWithShoppingCartSpecification(customerId);
            var CurrentCustomer = await CustomerRepository.GetByIdWithSpec(spec);
            var Shopping_ID = CurrentCustomer.ShoppingCart_ID;
            
            var shoppingCartItem = await ShoppingCartItemRepository.GetAllAsync([item => item.ItemId == Id && item.ShoppingCartId == Shopping_ID], includeProperties: "Item");

            ShoppingCartItemVM shoppingCartItemVM = new ShoppingCartItemVM();

            if (shoppingCartItem.Count != 0)
            {
                shoppingCartItemVM.Customer_ID = customerId;
                shoppingCartItemVM.Item = Item;
                shoppingCartItemVM.Quantity = shoppingCartItem[0].Quantity;
                shoppingCartItemVM.TotalPrice = shoppingCartItem[0].Quantity * Item.Price;
                return View(shoppingCartItemVM);

            }

            shoppingCartItemVM.Customer_ID = customerId;
            shoppingCartItemVM.Item = Item;
            ViewBag.ShoppingID = Shopping_ID;

            return View(shoppingCartItemVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] ShoppingCartItemVM shoppingCartItemVM)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Item = await itemRepository.GetByIdAsync(shoppingCartItemVM.Item_ID);

            CustomerWithShoppingCartSpecification spec = new CustomerWithShoppingCartSpecification(customerId);

            var CurrentCustomer = await CustomerRepository.GetByIdWithSpec(spec);

            var Shopping_ID = CurrentCustomer.ShoppingCart_ID;

            var shpooingCart = await ShoppingCartRepository.GetByIdAsync(Shopping_ID);

            if (ModelState.IsValid)
            {
                if (shpooingCart != null)
                {
                    var CurrentshoppingCartItem = await ShoppingCartItemRepository.GetAllAsync([item => item.ItemId == Item.Id && item.ShoppingCartId == Shopping_ID], includeProperties: "Item");
                    if (CurrentshoppingCartItem.Count != 0)
                    {
                        shpooingCart.TotalPrice -= CurrentshoppingCartItem[0].Quantity * Item.Price;
                        shpooingCart.TotalPrice += shoppingCartItemVM.TotalPrice;
                        CurrentshoppingCartItem[0].Quantity = shoppingCartItemVM.Quantity;

                        await ShoppingCartRepository.Update(shpooingCart);
                        await ShoppingCartItemRepository.Update(CurrentshoppingCartItem[0]);
                    }
                    else
                    {

                        if (shpooingCart.TotalPrice == null)
                        {
                            shpooingCart.TotalPrice = shoppingCartItemVM.TotalPrice;

                        }
                        else
                        {
                            shpooingCart.TotalPrice += shoppingCartItemVM.TotalPrice;

                        }

                        await ShoppingCartRepository.Update(shpooingCart);
                        ShoppingCartItem shoppingCartItem = new ShoppingCartItem
                        {
                            ItemId = Item.Id,
                            Quantity = shoppingCartItemVM.Quantity,
                            ShoppingCartId = Shopping_ID ?? 0
                        };
                        await ShoppingCartItemRepository.Add(shoppingCartItem);
                    }
                }


            }
            return View();
        }
        public async Task<IActionResult> CartCheckout()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerWithShoppingCartSpecification spec = new CustomerWithShoppingCartSpecification(customerId);
            var CurrentCustomer = await CustomerRepository.GetByIdWithSpec(spec);
            var Shopping_ID = CurrentCustomer.ShoppingCart_ID;
            var CurrentCShoppingCart = await ShoppingCartRepository.GetByIdAsync(Shopping_ID);
            var shoppingCartItem = await ShoppingCartItemRepository.GetAllAsync([item => item.ShoppingCartId == Shopping_ID]);
            ICollection<Item> _Items = new HashSet<Item>();
            List<int?> QuantityList = new List<int?>();
            List<int?> _SelectedItemS = new List<int?>();

            for (int i = 0; i < shoppingCartItem.Count; i++)
            {
                var item = await _itemRepository.GetByIdAsync(shoppingCartItem[i].ItemId);
                _SelectedItemS.Add(shoppingCartItem[i].ItemId);
                QuantityList.Add(shoppingCartItem[i].Quantity);
                _Items.Add(item);
            }
            CartVM cartVM = new CartVM { SelectedItemS = _SelectedItemS, Items = _Items, TotalPrice = CurrentCShoppingCart.TotalPrice, Quantity = QuantityList };
            return View(cartVM);
        }
        //[HttpPost]
        public async Task<IActionResult> ClearShoppingCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerWithShoppingCartSpecification spec = new CustomerWithShoppingCartSpecification(customerId);
            var CurrentCustomer = await CustomerRepository.GetByIdWithSpec(spec);
            var Shopping_ID = CurrentCustomer.ShoppingCart_ID;
            ShoppingCart shoppingCart = await ShoppingCartRepository.GetByIdAsync(Shopping_ID);
            var shoppingCartItem = await ShoppingCartItemRepository.GetAllAsync([item => item.ShoppingCartId == Shopping_ID], includeProperties: "Item");
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
            return View();
        }
        public async Task<IActionResult> DeleteItemFromShoppingCart(int Id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CustomerWithShoppingCartSpecification spec = new CustomerWithShoppingCartSpecification(customerId);
            var CurrentCustomer = await CustomerRepository.GetByIdWithSpec(spec);
            var Shopping_ID = CurrentCustomer.ShoppingCart_ID;
            ShoppingCart shoppingCart = await ShoppingCartRepository.GetByIdAsync(Shopping_ID);
            var shoppingCartItem = await ShoppingCartItemRepository.GetAllAsync([item => item.ItemId == Id && item.ShoppingCartId == Shopping_ID], includeProperties: "Item");

            var ExistsItem = await _itemRepository.GetByIdAsync(Id);
          
            if (shoppingCartItem.Count != 0)
            {
                    var item = shoppingCartItem[0];
                    await ShoppingCartItemRepository.Delete(item.ItemId, item.ShoppingCartId);
            }
            if (shoppingCart != null)
            {
                shoppingCart.TotalPrice -= ExistsItem.Price * shoppingCartItem[0].Quantity;
                await ShoppingCartRepository.Update(shoppingCart);
            }
            return RedirectToAction("CartCheckout");
        }
    }
}