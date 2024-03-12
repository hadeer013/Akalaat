using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> BuildQuery(IQueryable<T> InputQuery, ISpecification<T> Spec)
        {
            var query = InputQuery;

            if (Spec.Criteria != null)
                query = query.Where(Spec.Criteria);

            if (Spec.Includes != null)
                query = Spec.Includes.Aggregate(query,(current,include)=> current.Include(include));

            if (Spec.IncludeThenIncludes != null)
                query = Spec.IncludeThenIncludes.Aggregate(query,(current,include)=> include(current));
    
            return query;

        }
    }
}
