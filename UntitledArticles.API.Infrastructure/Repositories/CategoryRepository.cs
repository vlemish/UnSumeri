using Microsoft.EntityFrameworkCore;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.EntityFrameworkCore.DbSet<Category> _categories;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _categories = _context.Categories;
        }

        public async Task<Category> AddAsync(Category entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                await _categories.AddAsync(entity);
                await _context.SaveChangesAsync();
                var res = await GetOneByFilter(c => c.Name == entity.Name);
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(Category entity)
        {
            try
            {
                _categories.Remove(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteManyAsync(IReadOnlyCollection<Category> entities)
        {
            try
            {
                _categories.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _categories.ToListAsync();
        }

        public Task<int> GetCount(Func<Category, bool> predicate)
        {
            return _categories.Where(c => predicate(c)).CountAsync();
        }

        public async Task<IList<Category>> GetManyByFilter(Func<Category, bool> predicate)
        {
            return await _categories.Where(c => predicate(c)).ToListAsync();
        }

        public async Task<Category> GetOneByFilter(Func<Category, bool> predicate)
        {
            try
            {
                return _categories.Include(s => s.SubCategories).Where(predicate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Category> GetOneById(int id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task UpdateAsync(Category entity)
        {
            var entityToUpdate = await GetOneById(entity.Id);
            if (entity is null)
            {
                return;
            }

            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
