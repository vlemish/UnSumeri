using Microsoft.Extensions.Logging;

using System.Data.Entity;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Models;

namespace UntitledSelfArticles.API.Infrastructure
{
    public class AbstractRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ILogger<AbstractRepository<T>> _logger;

        private ApplicationDbContext _context;

        protected Microsoft.EntityFrameworkCore.DbSet<T> _table;

        public AbstractRepository(ILogger<AbstractRepository<T>> logger)
        {
            _context = new ApplicationDbContext();
            _table = _context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<T> Add(T entity)
        {
            try
            {
                return (await _table.AddAsync(entity)).Entity;
            }
            catch (Exception ex)
            {
                // logging;
                throw;
            }
        }

        public virtual async Task<T> Delete(int id)
        {
            try
            {
                var entity = await _table.FindAsync(id);
                if (entity is null)
                {
                    return null;
                }

                _table.Remove(entity);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while was trying to delete an entity: {ex.Message}", ex);
                throw;
            }
        }

        public virtual async Task<ICollection<T>> GetAll(LoadOptions loadOptions)
        {
            return await _table.Select(s => s).ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetByFilter(Predicate<T> filter, LoadOptions loadOptions)
        {
            var query = _table.Select(s => s)
                .Where(s => filter(s))
                .Skip(loadOptions.SkipRows);
            return loadOptions is null
                ? await query.ToListAsync()
                : await query.Take(loadOptions.MaxRows.Value).ToListAsync();
        }

        public virtual async Task<T> GetOneById(int id)
        {
            try
            {
                return await _table.FindAsync(id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occured while was trying to get an entity: {ex.Message}", ex);
                throw;
            }
        }

        public virtual bool Update(T entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return true;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
