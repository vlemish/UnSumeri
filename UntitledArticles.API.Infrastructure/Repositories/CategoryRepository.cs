using Microsoft.EntityFrameworkCore;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;
using UntitledArticles.API.Infrastructure.Extensions;

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

        #region Implementation of ICategoryRepository

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
                Category parent = entity;
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

        public async Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption) =>
            await GetAll(loadOptions, orderByOption, depth: 8);

        public async Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, int depth) =>
            await _categories
                .Sort(p => p.Id, orderByOption)
                .Where(c => !c.ParentId.HasValue)
                .Skip(loadOptions.Skip)
                .Take(loadOptions.Offset)
                .IncludeSelfReferencingCollectionWithDepth(c => c.SubCategories, depth)
                .Include(c => c.SubCategories)
                .ThenInclude(c => c.SubCategories)
                .AsNoTracking()
                .ToListAsync();


        public async Task<int> GetCount(Func<Category, bool> predicate)
        {
            return _categories.Count();
        }

        public async Task<IList<Category>> GetManyByFilter(Func<Category, bool> predicate) =>
            await GetManyByFilter(predicate, depth: 2);

        public async Task<IList<Category>> GetManyByFilter(Func<Category, bool> predicate, int depth) =>
            await _categories.IncludeSelfReferencingCollectionWithDepth(c => c.SubCategories, depth)
                .AsNoTracking()
                .Where(c => predicate(c))
                .ToListAsync();

        public async Task<Category> GetOneByFilter(Func<Category, bool> predicate)
        {
            try
            {
                return await GetOneByFilter(predicate, depth: 2);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Category> GetOneByFilter(Func<Category, bool> predicate, int depth)
        {
            return _categories.IncludeSelfReferencingCollectionWithDepth(c => c.SubCategories, depth)
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefault();
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

        #endregion
    }
}
