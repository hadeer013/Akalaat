using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.DistrictSpec
{
    public class DistrictWithRegionSpecification : BaseSpecification<District>
    {
        public DistrictWithRegionSpecification(int id) : base(d=>d.Id==id)
        {
            AddInclude(dis=>dis.regions);
        }
    }
}
