using System.Linq.Expressions;
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
                _categories.Remove(parent);
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

        public async Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, Expression<Func<Category, bool>> predicate) =>
            await GetAll(loadOptions, orderByOption, predicate, depth: 8);

        public async Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, Expression<Func<Category, bool>> predicate, int depth)
        {
            var categories = await _categories
                .Where(predicate)
                .Where(c => !c.ParentId.HasValue)
                .Skip(loadOptions.Skip)
                .Take(loadOptions.Offset)
                .IncludeSelfReferencingCollectionWithDepth(c => c.SubCategories, depth)
                .ThenInclude(c => c.Articles)
                .Sort(p=> p.Id, orderByOption)
                .AsNoTracking()
                .ToListAsync();

            foreach (var category in categories)
            {
                LoadCategories(category);
            }

            return categories;
        }

        private void LoadCategories(Category category)
        {
            if (category is null)
            {
                return;
            }
            var res = _categories
                .AsNoTracking()
                .Where(c => c.Id == category.Id)
                .Include(c => c.Articles)
                .FirstOrDefault()
                .Articles;
            category.Articles = res;
            if (category?.SubCategories is null || category?.SubCategories.Count == 0)
            {
                return;
            }

            foreach (var subCategory in category.SubCategories)
            {
                LoadCategories(subCategory);
            }
        }

        public async Task<int> GetCount(Expression<Func<Category, bool>> predicate)
        {
            return _categories.Count();
        }

        public async Task<IList<Category>> GetManyByFilter(Expression<Func<Category, bool>> predicate) =>
            await GetManyByFilter(predicate, depth: 2);

        public async Task<IList<Category>> GetManyByFilter(Expression<Func<Category, bool>> predicate, int depth)
        {
            var categories = await _categories
                .AsNoTracking()
                .Where(predicate)
                .IncludeSelfReferencingCollectionWithDepth(c => c.SubCategories, depth)
                .ThenInclude(c => c.Articles)
                .ToListAsync();
            foreach (var category in categories)
            {
                LoadCategories(category);
            }

            return categories;
        }

        public async Task<Category> GetOneByFilter(Expression<Func<Category, bool>> predicate)
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

        public async Task<Category> GetOneByFilter(Expression<Func<Category, bool>> predicate, int depth)
        {
            var category = await _categories
                .AsNoTracking()
                .Where(predicate)
                .IncludeSelfReferencingCollectionWithDepth(c => c.SubCategories, depth)
                .ThenInclude(c => c.Articles)
                .FirstOrDefaultAsync();

                LoadCategories(category);
                return category;
        }

        public async Task<Category> GetOneById(int id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task UpdateAsync(Category entity)
        {
            var entityToUpdate = await _categories.FindAsync(entity.Id, entity.UserId);
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
