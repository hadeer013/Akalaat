using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Region: BaseEntity
    {
        public string Name { get; set; }
        [ForeignKey("District")]
        public int? District_ID { get; set; }
        public virtual District District { get; set; }
        public ICollection<Branch> Branches { get; set; } = new HashSet<Branch>(); //branches that exist in this region(Address)
        public ICollection<Branch> BranchesDelivery { get; set; } = new HashSet<Branch>(); //These Branches can deliver to this region

        public ICollection<Address_Book> AddressBooks { get; set; } = new HashSet<Address_Book>();

    }
}
