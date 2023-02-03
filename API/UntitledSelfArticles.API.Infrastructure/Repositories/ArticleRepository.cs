using Microsoft.Extensions.Logging;

using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledSelfArticles.API.Infrastructure.Repositories
{
    public class ArticleRepository : AbstractRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ILogger<AbstractRepository<Article>> logger) : base(logger)
        {
        }
    }
}
