using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Data;
using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Repositories
{
    public class RegionRepository : GenericRepository<Region>, IRegionRepository
    {
        private readonly AkalaatDbContext context;

        public RegionRepository(AkalaatDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<Region>> GetAllRegionsByDistrictId(int DistrictId)
        {
            return await context.Regions.Where(r => r.District_ID == DistrictId).ToListAsync();
        }
    }
}
