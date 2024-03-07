using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Mood")]

    public class Mood
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public List<Resturant_Mood> resturant_Moods { get; set; } = new List<Resturant_Mood>();

    }
}
