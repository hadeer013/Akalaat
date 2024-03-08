using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Reservation: BaseEntity
    {

        public DateTime Start_Time { get; set; }

        public DateTime? End_Time { get; set; }

        [ForeignKey("Customer")]
        public string Customer_ID { get; set; }
        public virtual Customer Customer { get; set; }
        public ICollection<Branch> BranchReservations { get; set; } = new HashSet<Branch>();

    }
}
