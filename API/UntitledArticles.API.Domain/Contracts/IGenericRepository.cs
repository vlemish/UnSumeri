using UntitledArticles.API.Domain.Models;

namespace UntitledArticles.API.Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Add(T entity);

        bool Update(T entity);

        Task<T> Delete(int id);

        Task<T> GetOneById(int id);

        Task<ICollection<T>> GetByFilter(Predicate<T> filter, LoadOptions loadOptions);

        Task<ICollection<T>> GetAll(LoadOptions loadOptions);

        Task SaveChanges();
    }
}
