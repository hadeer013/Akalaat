using Akalaat.BLL.Interfaces;
using Akalaat.BLL.Specifications;
using Akalaat.DAL.Data;
using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AkalaatDbContext context;

        public GenericRepository(AkalaatDbContext context)
        {
            this.context = context;
        }

        public async Task<T> GetByIdWithRelatedAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(entity => (int)entity.GetType().GetProperty("Id").GetValue(entity) == id);
        }

        public async Task<T> Add(T type)
        {
            var entity = await context.Set<T>().AddAsync(type);
            await context.SaveChangesAsync();
            return entity.Entity;

		}

         public async Task<int> Delete<Y>(Y Id)
         {
             context.Set<T>().Remove(context.Set<T>().Find(Id));
             return await context.SaveChangesAsync();
         }
    

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync<Y>(Y Id)
        {
            return await context.Set<T>().FindAsync(Id);
        }

        public async Task<int> Update(T type)
        {
            //old code
            // context.Set<T>().Update(type);
            // return await context.SaveChangesAsync();
            
            var primaryKeyValues = await GetPrimaryKeyValues( type);

            // Search for an existing entity with the same primary key values
            var existingEntity = await context.Set<T>().FindAsync(primaryKeyValues);

            if (existingEntity != null)
            {
                // Detach the existing entity if found
                context.Entry(existingEntity).State = EntityState.Detached;
            }

            // Attach the new entity and mark it as modified
            context.Set<T>().Attach(type);
            context.Entry(type).State = EntityState.Modified;

            return await context.SaveChangesAsync();
           
        }




        /// Spec Pattern \\\

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.BuildQuery(context.Set<T>(), spec);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }


        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        
        //helper functions
        public  async Task  ClearTrackingAsync()
        {
            context.ChangeTracker.Clear();
        }
        public   async Task  BeginTransactionAsync()
        {
            if (context.Database.CurrentTransaction == null)
            {
                await context.Database.BeginTransactionAsync();
            }
        }

        

        public async Task CommitTransactionAsync()
        {
            await context.Database.CurrentTransaction?.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await context.Database.CurrentTransaction?.RollbackAsync();
        }
        public virtual async  Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<T> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties)
            
        {
            // Get the key property name
            var keyPropertyName = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.PropertyInfo.Name)
                .Single();

            // Create the expression for the key property
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, keyPropertyName);
            var equals = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

            // Build the query
            IQueryable<T> query = context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            // Execute the query
            return await query.FirstOrDefaultAsync(lambda);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(List<Expression<Func<T, bool>>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            // Apply filters
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            // Include properties
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Apply ordering
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }


            public async Task<object[]> GetPrimaryKeyValues<T>( T entity)
              
            {
                var entityEntry = context.Entry(entity);
                var primaryKey = entityEntry.Metadata.FindPrimaryKey();

                if (primaryKey.Properties.Count == 1)
                {
                    var primaryKeyProperty = primaryKey.Properties.First();
                    return new object[] { entityEntry.Property(primaryKeyProperty.Name).CurrentValue };
                }
                else if (primaryKey.Properties.Count > 1)
                {
                    var primaryKeyValues = new object[primaryKey.Properties.Count];
                    for (int i = 0; i < primaryKey.Properties.Count; i++)
                    {
                        var propertyName = primaryKey.Properties[i].Name;
                        primaryKeyValues[i] = entityEntry.Property(propertyName).CurrentValue;
                    }
                    return primaryKeyValues;
                }
                else
                {
                    throw new InvalidOperationException("Entity does not have a primary key.");
                }
            }
            public async Task<int> Delete(int? ItemID,int? ShoppingID)
            {
                var entity =  context.Set<ShoppingCartItem>().FirstOrDefault(e => e.ItemId == ItemID && e.ShoppingCartId == ShoppingID);;
                if (entity != null)
                {
                    context.Set<ShoppingCartItem>().Remove(entity);
                    return await context.SaveChangesAsync();
                }
                return 0; 
            }

        
    }
    
}
