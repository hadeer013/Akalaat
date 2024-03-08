using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Resturant: BaseEntity
    {

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
        public ICollection<Review> reviews { get; set; } = new HashSet<Review>();
        public ICollection<Dish> Dishes { get; set; }=new HashSet<Dish>();
        public ICollection<Mood> Moods { get; set; }=new HashSet<Mood>();
        public ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();

    }
}
