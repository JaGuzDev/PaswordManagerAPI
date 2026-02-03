using System.Linq.Expressions;

namespace PasswordManager.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(object id);
        Task<IList<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> items);
        Task UpdateRangeAsync(IEnumerable<T> items);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        // Queryable for LINQ
        IQueryable<T> GetAllQueryable();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Raw SQL support (EF Core)
        //IQueryable<T> FromSqlRaw(string sql, params object[] parameters);

        // Max/Min by selector
        Task<TResult?> MaxAsync<TResult>(Expression<Func<T, TResult>> selector);
        Task<TResult?> MinAsync<TResult>(Expression<Func<T, TResult>> selector);

        // Synchronous overloads (not recommended, but for compatibility)
        T? GetById(int id);
        T? GetById(string id);
        T? GetById(long id);
    }
}
