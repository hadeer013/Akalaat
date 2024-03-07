using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Reservation")]
    public class Reservation
    {
        public int ID { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime? End_Time { get; set; }

        [ForeignKey("Customer")]
        public int Customer_ID { get; set; }
        public virtual Customer Customer { get; set; }
        public List<Branch_Reservation> branch_Reservations { get; set; } = new List<Branch_Reservation>();

    }
}
