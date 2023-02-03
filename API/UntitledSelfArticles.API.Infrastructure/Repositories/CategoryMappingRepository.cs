using Microsoft.Extensions.Logging;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledSelfArticles.API.Infrastructure.Repositories
{
    public class CategoryMappingRepository : AbstractRepository<CategoryMapping>, ICategoryMappingRepository
    {
        public CategoryMappingRepository(ILogger<AbstractRepository<CategoryMapping>> logger) : base(logger)
        {
        }
    }
}
