using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }//where
        public List<Expression<Func<T, object>>> Includes { get; set; }//include




        List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> IncludeThenIncludes { get; set; }

    }
}
