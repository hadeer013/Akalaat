using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{

    public class Item_Size: BaseEntity
    {
        public int Size { get; set; }
        public List<Menu_Item_Size> menu_Item_Sizes { get; set; } = new List<Menu_Item_Size>();

    }
}
