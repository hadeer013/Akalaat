using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Extra: BaseEntity
    {
        public string Name { get; set; }
        [ForeignKey("Item")]
        public int? Item_ID { get; set; }
        public virtual Item Item { get; set; }

    }
}
