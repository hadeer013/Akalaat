using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akalaat.ViewModels
{
    public class ShoppingCartItemVM
    {
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public string? Customer_ID { get; set; }
        public virtual Customer? Customer { get; set; }
        //   [Display(Name = "Items")]
        //  public List<int> SelectedItemS { get; set; } = new List<int>();

        // public IEnumerable<SelectListItem> Items { get; set; } = Enumerable.Empty<SelectListItem>();
        public Item? Item { get; set; }
        public int? Item_ID { get; set; }

        public int? Quantity { get; set; }
        public float? TotalPrice { get; set; }
    }
}
