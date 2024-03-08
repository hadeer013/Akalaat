using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Data;
using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AkalaatDbContext context;

        public GenericRepository(AkalaatDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Add(T type)
        {
            await context.Set<T>().AddAsync(type);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(int Id)
        {
            context.Set<T>().Remove(context.Set<T>().Find(Id));
            return await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await context.Set<T>().FindAsync(Id);
        }

        public async Task<int> Update(T type)
        {
            context.Set<T>().Update(type);
            return await context.SaveChangesAsync();
        }
    }
}
