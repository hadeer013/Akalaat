using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.ResturantSpec
{
    public class ResturantParams
    {
        public string sort {  get; set; }
        public List<int> dishId {  get; set; }
        public int? RegionId {  get; set; }
        public string RestaurantName {  get; set; }

    }
}
