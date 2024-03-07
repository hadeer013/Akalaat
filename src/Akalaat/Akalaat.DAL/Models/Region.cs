using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Region")]
    public class Region
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("District")]
        public int? District_ID { get; set; }
        public virtual District District { get; set; }
        public List<Branch> branches { get; set; } = new List<Branch>();
        public List<Available_Delivery_Area> delivery_Areas { get; set; } = new List<Available_Delivery_Area>();

        public List<Address_Book> address_Books { get; set; } = new List<Address_Book>();

    }
}
