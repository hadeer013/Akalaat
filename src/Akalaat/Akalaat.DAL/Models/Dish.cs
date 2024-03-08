using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Dish: BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public string Dish_image { get; set; }
        public ICollection<Resturant> resturantDishes { get; set; } = new HashSet<Resturant>();

    }
}
