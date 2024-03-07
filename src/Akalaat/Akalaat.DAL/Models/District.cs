using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("District")]

    public class District
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("City")]
        public int? City_ID { get; set; }
        public virtual City City { get; set; }
        public List<Region> regions { get; set; } = new List<Region>();

    }
}
