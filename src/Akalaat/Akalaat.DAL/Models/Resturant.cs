using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Resturant")]

    public class Resturant
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
        public string Name { get; set; }

        [Required]
        public string Logo_URL { get; set; }

        [Required]
        public string Cover_URL { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [ForeignKey("Vendor")]
        public string Vendor_ID { get; set; }
        public virtual Vendor Vendor { get; set; }

        [ForeignKey("Menu")]
        public int? Menu_ID { get; set; }
        public virtual Menu Menu { get; set; }
        public List<Review> reviews { get; set; } = new List<Review>();
        public List<Resturant_Dish> resturant_Dishes { get; set; } = new List<Resturant_Dish>();
        public List<Resturant_Mood> resturant_Moods { get; set; } = new List<Resturant_Mood>();
        public List<Branch> branches { get; set; } = new List<Branch>();

    }
}
