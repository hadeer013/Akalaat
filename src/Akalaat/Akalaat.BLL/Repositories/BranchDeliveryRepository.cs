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
			return await context.AvailableDeliveryAreas.Where(d=>d.BranchId==BranchId && d.RegionId==RegionId).FirstOrDefaultAsync();
        }
    }
}
