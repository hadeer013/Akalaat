using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Offer: BaseEntity
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        public string Name { get; set; }

        [Required]
        public string Offer_image { get; set; }

        public string Description { get; set; }

        [ForeignKey("Menu")]
        public int Menu_ID { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual ICollection<Item> Items { get; set; }=new HashSet<Item>();
        //public List<Items_in_Offer> items_In_Offers { get; set; } = new List<Items_in_Offer>();


    }
}
