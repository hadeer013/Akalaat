using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Menu_Item_Size
    {
        [ForeignKey("Item")]

        public int Item_ID { get; set; }
        public virtual Item Item { get; set; }

        [ForeignKey("Item_Size")]
        public int Item_Size_ID { get; set; }
        public virtual Item_Size Item_Size { get; set; }

        public string Size_Image_URL { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
