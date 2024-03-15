using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Review: BaseEntity
    {

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public int No_of_Likes { get; set; }
        public string? ReviewImage { get; set; }

        [ForeignKey("Customer")]
        public string Customer_ID { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Resturant")]
        public int? Resturant_ID { get; set; }
        public virtual Resturant Resturant { get; set; }
    }
}
