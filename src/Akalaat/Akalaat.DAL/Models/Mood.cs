using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Mood: BaseEntity
    {

        [Required]
        public string Name { get; set; }
        public ICollection<Resturant> resturant_Moods { get; set; } = new HashSet<Resturant>();

    }
}
