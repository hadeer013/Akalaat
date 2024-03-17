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
    public class BranchDeliveryRepository : IBranchDeliveryRepository
    {
        private readonly AkalaatDbContext context;

        public BranchDeliveryRepository(AkalaatDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddBranchDeliveryArea(Available_Delivery_Area delivery_Area)
        {
            await context.AvailableDeliveryAreas.AddAsync(delivery_Area);
            return await context.SaveChangesAsync();
        }


        public async Task<int> DeleteBranchDeliveryArea(Available_Delivery_Area delivery_Area)
        {
            context.AvailableDeliveryAreas.Remove(delivery_Area);
            return await context.SaveChangesAsync();
        }

        public async Task<Available_Delivery_Area> GetBranchDeliveryArea(int BranchId, int RegionId)
        {
            return await context.AvailableDeliveryAreas.Where(d => d.BranchId == BranchId && d.RegionId == RegionId).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteAllDeliveryAreas(int BranchId)
        {
            foreach (var item in context.AvailableDeliveryAreas.Where(av => av.BranchId == BranchId))
            {
                context.AvailableDeliveryAreas.Remove(item);
            }
            return await context.SaveChangesAsync();
        }

        //important concideration

        public async Task<IReadOnlyList<City>> GetAllCitesAvailableAsDeliveryAreas(int BranchId)
        {
            var allRegionsInBranch = context.AvailableDeliveryAreas
                .Where(a => a.BranchId == BranchId)
                .Select(a => a.RegionId);

            return await context.Regions.Where(r => !allRegionsInBranch
                                        .Contains(r.Id))
                                        .Include(r => r.District)
                                        .ThenInclude(d => d.City)
                                        .Select(r => r.District.City)
                                        .Distinct()
                                        .ToListAsync();
        }

        public async Task<IReadOnlyList<District>> GetAllDistrictsAvailableAsDeliveryAreas(int BranchId, int CityId)
        {
            var allRegionsInBranch = context.AvailableDeliveryAreas
                .Where(a => a.BranchId == BranchId)
                .Select(a => a.RegionId);

            var result = await context.Regions.Where(r => !allRegionsInBranch
                .Contains(r.Id))
                .Include(r => r.District)
                .ThenInclude(d => d.City)
                .Where(d => d.District.City_ID == CityId)
                .Select(r => r.District)
                .Distinct()
                .ToListAsync();
            return result;
        }

        public async Task<IReadOnlyList<Region>> GetAllRegionAvailableAsDeliveryAreas(int BranchId, int DistrictId)
        {
            var allRegionsInBranch = context.AvailableDeliveryAreas
                .Where(a => a.BranchId == BranchId)
                .Select(a => a.RegionId);


            var result = await context.Regions.Where(r => !allRegionsInBranch
                                            .Contains(r.Id))
                                            .Where(r => r.District_ID == DistrictId)
                                            .ToListAsync();


            return result;
        }
    }
}
