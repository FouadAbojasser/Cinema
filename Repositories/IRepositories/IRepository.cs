using Cinema.Models;
using System.Linq.Expressions;

namespace Cinema.Repositories.IRepositories
{
    public interface IRepository <T> where T : class
    {
        Task<bool> CreateAsync(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        T? GetOne(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool NoTracking = true
            );

        IEnumerable<T> Get(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? includes = null,
            bool NoTracking = true,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
            );

        Task CommitAsync();

    }
}
