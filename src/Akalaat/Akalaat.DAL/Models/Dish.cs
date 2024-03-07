using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Dish")]

    public class Dish
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Dish_image { get; set; }
        public List<Resturant_Dish> resturant_Dishes { get; set; } = new List<Resturant_Dish>();

    }
}
