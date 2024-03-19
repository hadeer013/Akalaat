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
        [ForeignKey("ShoppingCart")]
        public int? ShoppingCart_ID { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }

        public ICollection<Reservation> reservations { get; set; } = new HashSet<Reservation>();
        public virtual Address_Book? Address_Book { get; set; }
    }
}
