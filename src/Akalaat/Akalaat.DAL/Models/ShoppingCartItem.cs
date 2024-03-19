using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class ShoppingCartItem
    {
        public int? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }

        public int? ItemId { get; set; }
        public Item? Item { get; set; }

        public int? Quantity { get; set; }
    }
}
