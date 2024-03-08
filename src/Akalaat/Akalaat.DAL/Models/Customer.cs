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
        public List<Review> reviews { get; set; } = new List<Review>();
        public List<Order> orders { get; set; } = new List<Order>();
        public List<Reservation> reservations { get; set; } = new List<Reservation>();

        [ForeignKey("Address_Book")]
        public int? Address_Book_ID { get; set; }
        public virtual Address_Book Address_Book { get; set; }
    }
}
