using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Order: BaseEntity
    {

        public DateTime DateTime { get; set; }

        public DateTime Arrival_Time { get; set; }

        public decimal Total_Price { get; set; }

        public decimal Total_Discount { get; set; }
        [ForeignKey("Customer")]
        public string Customer_ID { get; set; }
        public virtual Customer? Customer { get; set; }
        //[ForeignKey("Item")]
        //public int? Item_ID { get; set; }
        //public virtual Item Item { get; set; }
        //  public ICollection<OrderItem> OrderItems { get; set; }= new HashSet<OrderItem>();
        public ICollection<Item> Items { get; set; } = new HashSet<Item>();

    }
}
