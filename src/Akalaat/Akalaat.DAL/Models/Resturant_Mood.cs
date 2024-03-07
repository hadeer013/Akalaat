using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Resturant_Mood")]

    public class Resturant_Mood
    {
        [ForeignKey("Resturant")]
        public int Resturant_ID { get; set; }
        public virtual Resturant Resturant { get; set; }

        [ForeignKey("Mood")]
        public int Mood_ID { get; set; }
        public virtual Mood Mood { get; set; }
    }
}
