using Akalaat.BLL.Specifications;
using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Add(T type);
        Task<int> Update(T type);
        Task<int> Delete<Y>(Y Id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync<Y>(Y Id);

        /// Spec Pattern \\\

        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<T> GetByIdWithSpec(ISpecification<T> spec);
        
        ///helper functions
        
        Task  ClearTrackingAsync();
        Task  BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdIncludingAsync(int id, params Expression<Func<T, object>>[] includeProperties);
    }
}
