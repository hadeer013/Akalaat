using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<int> Add(T type);
        Task<int> Update(T type);
        Task<int> Delete(int Id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
    }
}
