using System.Linq.Expressions;
using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitledArticles.API.Domain.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, Expression<Func<Category, bool>> predicate, int depth);

        Task<IList<Category>> GetManyByFilter(Expression<Func<Category, bool>> predicate, int depth);

        Task<Category> GetOneByFilter(Expression<Func<Category, bool>> predicate, int depth);
    }
}
