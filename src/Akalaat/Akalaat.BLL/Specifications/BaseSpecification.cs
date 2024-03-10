﻿using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> IncludeThenIncludes { get; set; }

        public BaseSpecification() //for get All
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)//for get by id
        {
            this.Criteria = criteria;
        }
        public void AddInclude(Expression<Func<T, object>> Include)
        {

            Includes.Add(Include);
        }
    }
}