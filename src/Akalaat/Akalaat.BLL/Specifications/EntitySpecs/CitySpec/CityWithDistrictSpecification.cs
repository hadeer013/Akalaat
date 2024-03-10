using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.CitySpec
{
    public class CityWithDistrictSpecification : BaseSpecification<City>
    {
		//getAll cities with specified name
		public CityWithDistrictSpecification(string CityName):base(c=> string.IsNullOrEmpty(CityName) ||
            c.Name.Contains(CityName))
		{
		}

		public CityWithDistrictSpecification(int id) : base(city => city.Id == id) //get with id
        {
            AddInclude(city => city.Districts);
        }
    }
}
