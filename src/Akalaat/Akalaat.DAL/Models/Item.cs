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

        public int Likes { get; set; }

        public string Description { get; set; }

        [Required]
        public string Image_URL { get; set; }

        public decimal Discount { get; set; }

        public bool IsOffer { get; set; }
        public ICollection<Menu_Item_Size> menu_Item_Sizes { get; set; } = new HashSet<Menu_Item_Size>();
        public ICollection<Extra> extras { get; set; } = new HashSet<Extra>();


    }
}
