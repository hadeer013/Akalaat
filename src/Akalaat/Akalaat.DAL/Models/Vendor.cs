using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Vendor")]
    public class Vendor:ApplicationUser
    {
        [ForeignKey("Resturant")]

        public int? Resturant_ID { get; set; }
        public virtual Resturant Resturant { get; set; }
    }
}
