using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Items_in_Offer")]

    public class Items_in_Offer
    {
        [ForeignKey("Offer")]
        public int Offer_ID { get; set; }
        public virtual Offer Offer { get; set; }

        [ForeignKey("Item")]
        public int Item_ID { get; set; }
        public virtual Item Item { get; set; }
    }
}
