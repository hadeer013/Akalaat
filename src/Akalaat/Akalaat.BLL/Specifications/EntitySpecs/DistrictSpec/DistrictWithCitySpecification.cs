using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.DistrictSpec
{
	public class DistrictWithCitySpecification : BaseSpecification<District>
	{
		//id=> city Id
		public DistrictWithCitySpecification(int Cityid,string DistrictName) : base(d=>d.City_ID==Cityid && (
		string.IsNullOrEmpty(DistrictName) || (d.Name.Contains(DistrictName))))
		{
		}
	}
}
