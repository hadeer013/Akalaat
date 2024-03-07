using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Menu")]

    public class Menu
    {
        [Key]
        public int ID { get; set; }
        public List<Item> items { get; set; } = new List<Item>();
        public int? Resturant_ID { get; set; }
        public virtual Resturant Resturant {get; set; }
        public List<Offer> offers { get; set; } = new List<Offer>();
        public List<Category> categories { get; set; } = new List<Category>();

    }
}
