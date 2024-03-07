using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Available_Delivery_Area
    {
        [ForeignKey("Region")]
        public int Region_ID { get; set; }
        public virtual Region Region { get; set; }

        [ForeignKey("Branch")]
        public int Branch_ID { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
