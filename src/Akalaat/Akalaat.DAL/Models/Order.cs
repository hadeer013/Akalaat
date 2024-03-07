using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Order")]

    public class Order
    {
        [Key]
        public int ID { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime Arrival_Time { get; set; }

        public decimal Total_Price { get; set; }

        public decimal Total_Discount { get; set; }
        [ForeignKey("Customer")]
        public int Customer_ID { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("Item")]
        public int? Item_ID { get; set; }
        public virtual Item Item { get; set; }
    }
}
