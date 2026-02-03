using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure.Data;
using System.Linq.Expressions;

namespace PasswordManager.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public AppDbContext DbContext => _dbContext;

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            // Returns all entities as a queryable, for further LINQ operations.
            return _dbSet.AsNoTracking();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            // Finds the first entity matching the predicate, or null if none found.
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        //public IQueryable<T> FromSqlRaw(string sql, params object[] parameters)
        //{
        //    // Executes a raw SQL query and returns the results as a queryable.
        //    // Only use this for SELECT queries!
        //    return _dbSet.FromSqlRaw(sql, parameters).AsNoTracking();
        //}

        public async Task<TResult?> MaxAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            // Returns the max value for the given selector, or default if empty.
            return await _dbSet.MaxAsync(selector);
        }

        public async Task<TResult?> MinAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            // Returns the min value for the given selector, or default if empty.
            return await _dbSet.MinAsync(selector);
        }

        public T? GetById(int id)
        {
            // Synchronous primary key lookup (not recommended for async code, but provided for compatibility)
            return _dbSet.Find(id);
        }

        public T? GetById(string id)
        {
            return _dbSet.Find(id);
        }

        public T? GetById(long id)
        {
            return _dbSet.Find(id);
        }
    }
}
