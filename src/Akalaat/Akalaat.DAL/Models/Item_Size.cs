using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Item_Size")]

    public class Item_Size
    {
        public int ID { get; set; }
        public int Size { get; set; }
        public List<Menu_Item_Size> menu_Item_Sizes { get; set; } = new List<Menu_Item_Size>();

    }
}
