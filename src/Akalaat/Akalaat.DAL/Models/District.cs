using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{

    public class District: BaseEntity
    {
        public string Name { get; set; }
        [ForeignKey("City")]
        public int City_ID { get; set; }
        public virtual City City { get; set; }
        public ICollection<Region> regions { get; set; } = new HashSet<Region>();

    }
}
