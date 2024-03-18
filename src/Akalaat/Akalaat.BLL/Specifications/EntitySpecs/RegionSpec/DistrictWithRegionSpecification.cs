using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.RegionSpec
{
    public class DistrictWithRegionSpecification : BaseSpecification<Region> //much care to this
    {
        public DistrictWithRegionSpecification(int id, string? RegionName) : base(r => r.District_ID == id && (
            string.IsNullOrEmpty(RegionName) || r.Name.Contains(RegionName)))
        {
            // AddInclude(dis => dis.District);
            AddThenInclude(dis => dis.Include(r => r.District));
        }

        public DistrictWithRegionSpecification(int RegionId):base(r=>r.Id==RegionId)
        {
            AddThenInclude(reg => reg.Include(r => r.District).ThenInclude(r=>r.City));
        }



    }
}
