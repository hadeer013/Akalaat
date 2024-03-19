using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public float? TotalPrice { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
     //   public int Quantity { get; set; }
    }
}
