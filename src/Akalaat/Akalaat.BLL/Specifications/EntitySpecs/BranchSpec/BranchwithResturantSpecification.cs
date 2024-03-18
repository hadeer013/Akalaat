using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.BranchSpec
{
    public class BranchwithResturantSpecification : BaseSpecification<Branch>
    {
        public BranchwithResturantSpecification(int Id,string Name="") : base(b=>b.Resturant_ID==Id && 
        (b.Region.Name.Contains(Name) || b.Region.District.Name.Contains(Name)))
        {
            IncludeThenIncludes.Add(b=>b.Include(brn=>brn.DeliveryAreas).Include(brn=>brn.Region).ThenInclude(brn=>brn.District).ThenInclude(d=>d.City));
        }

        public BranchwithResturantSpecification(int Id) : base(b => b.Id == Id)
        {
            IncludeThenIncludes.Add(b => b.Include(brn => brn.DeliveryAreas).ThenInclude(d=>d.District).ThenInclude(d=>d.City));
        }
    }
}
