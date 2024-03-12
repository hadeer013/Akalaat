using Akalaat.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akalaat.ViewModels
{
    public class OrderVM
    {
        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [Display(Name = "Arrival Time")]
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive number.")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Total Discount")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Total discount must be a positive number.")]
        public decimal TotalDiscount { get; set; }
        public string Customer_ID { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("Item")]
        public int? Item_ID { get; set; }
        public virtual Item Item { get; set; }

    }
}
