using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.ResturantSpec
{
	public class ResturantWithDishSpecification : BaseSpecification<Resturant>
	{
		public ResturantWithDishSpecification(string sort,List<int>? dish,int? regionId,string ResturantName):base(r=>
			( ResturantName.IsNullOrEmpty() || r.Name.Contains(ResturantName)) && 
		(r.Branches.Any(b=>b.DeliveryAreas.Any(d=>d.Id== regionId))) && dish==null || r.Dishes.Any(d=> dish.Contains(d.Id)))
		{
			AddThenInclude(r => r.Include(Res => Res.Dishes));
			AddThenInclude(r => r.Include(Res=>Res.Branches).ThenInclude(b=>b.DeliveryAreas));

			if(!sort.IsNullOrEmpty())
				OrderByDesc = O => O.Rating;

		}
	}
}
