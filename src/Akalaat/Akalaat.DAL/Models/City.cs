using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class City: BaseEntity
    {
        public string Name { get; set; }
        public ICollection<District> Districts { get; set; } = new HashSet<District>();

    }
}
