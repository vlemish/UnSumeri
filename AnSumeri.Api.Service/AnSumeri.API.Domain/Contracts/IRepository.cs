using System.Linq.Expressions;
using AnSumeri.API.Domain.Entities;
using AnSumeri.API.Domain.Enums;
using AnSumeri.API.Domain.Pagination;

namespace AnSumeri.API.Domain.Contracts
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

        Task<IList<T>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, Expression<Func<T, bool>> predicate);
    }
}
