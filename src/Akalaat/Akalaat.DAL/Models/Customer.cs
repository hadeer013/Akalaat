using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("Customer")]
    public class Customer : ApplicationUser
    {
        public ICollection<Review> reviews { get; set; } = new HashSet<Review>();
        public ICollection<Order> orders { get; set; } = new HashSet<Order>();
        public ICollection<Reservation> reservations { get; set; } = new HashSet<Reservation>();

        [ForeignKey("Address_Book")]
        public int? Address_Book_ID { get; set; }
        public virtual Address_Book Address_Book { get; set; }
    }
}
