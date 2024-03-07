using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Resturant_Dish")]

    public class Resturant_Dish
    {

        [ForeignKey("Resturant")]
        public int Resturant_ID { get; set; }
        public virtual Resturant Resturant { get; set; }

        [ForeignKey("Dish")]
        public int Dish_ID { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
