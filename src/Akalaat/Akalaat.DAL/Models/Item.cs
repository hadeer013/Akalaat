using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{

    public class Item: BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public int? Likes { get; set; }

        public string Description { get; set; }

        [Required]
        public string Image_URL { get; set; }

        public decimal? Discount { get; set; }

        public OfferStatus IsOffer { get; set; }
        [ForeignKey("Menu")]
        public int MenuID { get; set; }
        public virtual Menu menu { get; set; }
        
        [ForeignKey("Category")]

        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<Menu_Item_Size> menu_Item_Sizes { get; set; } = new HashSet<Menu_Item_Size>();
        public ICollection<Extra> extras { get; set; } = new HashSet<Extra>();


    }
    public enum OfferStatus
    {
        Regular,
        Offer
    }
}
