using UntitledArticles.API.Domain.Entities;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitledArticles.API.Domain.Contracts
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IList<Category>> GetAll(LoadOptions loadOptions, OrderByOption orderByOption, int depth);

        Task<IList<Category>> GetManyByFilter(Func<Category, bool> predicate, int depth);

        Task<Category> GetOneByFilter(Func<Category, bool> predicate, int depth);
    }
}
