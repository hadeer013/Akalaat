using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Item")]

    public class Item
    {
        [Key]
        public int Item_ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int Likes { get; set; }

        public string Description { get; set; }

        [Required]
        public string Image_URL { get; set; }

        public decimal Discount { get; set; }

        public bool IsOffer { get; set; }
        public List<Items_in_Offer> items_In_Offers { get; set; } = new List<Items_in_Offer>();
        public List<Menu_Item_Size> menu_Item_Sizes { get; set; } = new List<Menu_Item_Size>();
        public List<Extra> extras { get; set; } = new List<Extra>();


    }
}
