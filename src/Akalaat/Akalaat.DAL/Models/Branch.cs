using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{


    public class Branch: BaseEntity
    {
        public bool IsDineIn { get; set; }

        public bool IsDelivery { get; set; }

        public DateTime Open_Hour { get; set; }

        public DateTime Close_Hour { get; set; }

        public int Estimated_Delivery_Time { get; set; }
        [ForeignKey("Resturant")]
        public int Resturant_ID { get; set; }
        public virtual Resturant Resturant { get; set; }

        [ForeignKey("Region")]
        public int Region_ID { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<Region> DeliveryAreas { get; set; }=new HashSet<Region>();

    }
}
