using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.BranchSpec
{
    public class BranchWithitsLocationSpecification : BaseSpecification<Branch>
    {
        public BranchWithitsLocationSpecification(int Id) : base(b => b.Id == Id)
        {
            IncludeThenIncludes.Add(b => b.Include(brn => brn.Region).ThenInclude(d => d.District).ThenInclude(d => d.City));
        }
    }
}
