using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Branch_Reservation")]

    public class Branch_Reservation
    {
        [ForeignKey("Reservation")]
        public int Reservation_ID { get; set; }
        public virtual Reservation Reservation { get; set; }

        [ForeignKey("Branch")]
        public int Branch_ID { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
