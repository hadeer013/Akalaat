using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    [Table("City")]

    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<District> districts { get; set; } = new List<District>();

    }
}
