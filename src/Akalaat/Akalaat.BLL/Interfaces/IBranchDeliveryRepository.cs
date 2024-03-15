﻿using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Interfaces
{
	public interface IBranchDeliveryRepository
	{
		Task<int> AddBranchDeliveryArea(Available_Delivery_Area delivery_Area);
		Task<int> DeleteBranchDeliveryArea(Available_Delivery_Area delivery_Area);

		Task<Available_Delivery_Area> GetBranchDeliveryArea(int BranchId,int RegionId);
	}
}
