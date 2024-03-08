using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{
    public class Available_Delivery_Area
    {
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
