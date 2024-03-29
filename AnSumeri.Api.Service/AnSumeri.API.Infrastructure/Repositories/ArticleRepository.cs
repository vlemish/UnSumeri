using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;
using AnSumeri.API.Domain.Enums;
using AnSumeri.API.Domain.Pagination;

namespace AnSumeri.API.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.EntityFrameworkCore.DbSet<Article> _articles;

        public ArticleRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _articles = _context.Articles;
        }

        public async Task<Article> AddAsync(Article entity)
        {
            if (entity is null)
            {
                return null;
            }

            await _articles.AddAsync(entity);
            await _context.SaveChangesAsync();
            Article addedEntity =
                await GetOneByFilter(c =>
                    c.Title == entity.Title && c.CategoryId == entity.CategoryId &&
                    c.CreatedAtTime == entity.CreatedAtTime);
            return addedEntity;
        }

        public async Task<int> DeleteAsync(Article entity)
        {
            try
            {
                Article parent = entity;
                this._articles.Remove(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteManyAsync(IReadOnlyCollection<Article> entities)
        {
            try
            {
                this._articles.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Article>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption,
            Expression<Func<Article, bool>> predicate) =>
            await this._articles.AsNoTracking()
                .Where(predicate)
                .ToListAsync();

        public Task<int> GetCount(Expression<Func<Article, bool>> predicate) => this._articles.CountAsync(predicate);

        public async Task<IList<Article>> GetManyByFilter(Expression<Func<Article, bool>> predicate) =>
            await this._articles.AsNoTracking()
                .Where(predicate)
                .ToListAsync();

        public Task<Article> GetOneByFilter(Expression<Func<Article, bool>> predicate) =>
            _articles.AsNoTracking()
                .Where(predicate)
                .Include(a => a.Category)
                .FirstOrDefaultAsync();

        public async Task<Article> GetOneById(int id) =>
            await this._articles.AsNoTracking().FirstAsync(p=> p.Id == id);

        public async Task UpdateAsync(Article entity)
        {
            var entityToUpdate = await GetOneById(entity.Id);
            if (entity is null)
            {
                return;
            }

            _context.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
