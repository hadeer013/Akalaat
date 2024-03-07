using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Address_Book")]

    public class Address_Book
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Customer")]
        public string Customer_ID { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("Region")]
        public int Region_ID { get; set; }
        public virtual Region Region { get; set; }
    }
}
