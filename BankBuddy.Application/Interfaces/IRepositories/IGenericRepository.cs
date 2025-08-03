using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Interfaces.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        Task<List<T>> FindAllAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        IQueryable<T> Query();
        void Remove(T entity);
        Task SaveChangesAsync();
        void Update(T entity);
    }
}
