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
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        private readonly AkalaatDbContext context;

        public DistrictRepository(AkalaatDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<District>> GetAllDistrictsByCityId(int CityId)
        {
            return await context.Districts.Where(d => d.City_ID == CityId).ToListAsync();
        }
    }
}
