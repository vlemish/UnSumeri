using System.Linq.Expressions;
using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitledArticles.API.Domain.Contracts
{
    public interface IRepository<T> where T: IEntity
    {
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        Task DeleteManyAsync(IReadOnlyCollection<T> entities);

        Task<int> GetCount(Expression<Func<T, bool>> predicate);

        Task<T> GetOneById(int id);

        Task<T> GetOneByFilter(Expression<Func<T, bool>>predicate);

        Task<IList<T>> GetManyByFilter(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption);
    }
}
