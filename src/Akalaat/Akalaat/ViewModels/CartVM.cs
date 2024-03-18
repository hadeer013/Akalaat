using Akalaat.DAL.Models;

namespace Akalaat.ViewModels
{
    public class CartVM
    {
        public ICollection<Item> Items { get; set; }=new List<Item>();
        public float? TotalPrice { get; set; }

        public List<int?> Quantity = new List<int?>();
        public List<int?> SelectedItemS { get; set; } = new List<int?>();

    }
}
