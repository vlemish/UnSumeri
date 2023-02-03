using Microsoft.Extensions.Logging;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledSelfArticles.API.Infrastructure.Repositories
{
    public class CategoryRepository : AbstractRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ILogger<AbstractRepository<Category>> logger) : base(logger)
        {
        }
    }
}
