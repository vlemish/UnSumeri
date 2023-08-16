using System.Linq.Expressions;
using AnSumeri.API.Domain.Entities;
using AnSumeri.API.Domain.Enums;
using AnSumeri.API.Domain.Pagination;

namespace AnSumeri.API.Domain.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, Expression<Func<Category, bool>> predicate, int depth);

        Task<IList<Category>> GetManyByFilter(Expression<Func<Category, bool>> predicate, int depth);

        Task<Category> GetOneByFilter(Expression<Func<Category, bool>> predicate, int depth);
    }
}
