using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitledArticles.API.Infrastructure.Repositories
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
                await GetOneByFilter(c=> c.Title == entity.Title && c.CategoryId == entity.CategoryId && c.CreatedAtTime == entity.CreatedAtTime);
            return addedEntity;
        }
        public Task<int> DeleteAsync(Article entity) => throw new NotImplementedException();
        public Task DeleteManyAsync(IReadOnlyCollection<Article> entities) => throw new NotImplementedException();
        public Task<IList<Article>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption) => throw new NotImplementedException();
        public Task<int> GetCount(Expression<Func<Article, bool>> predicate) => throw new NotImplementedException();
        public Task<IList<Article>> GetManyByFilter(Expression<Func<Article, bool>> predicate) => throw new NotImplementedException();
        public Task<Article> GetOneByFilter(Expression<Func<Article, bool>> predicate) =>
            _articles.AsNoTracking()
            .Where(predicate)
            .Include(a => a.Category)
            .FirstOrDefaultAsync();

        public async Task<Article> GetOneById(int id) =>
            await this._articles.FindAsync(id);

        public async Task UpdateAsync(Article entity)
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
