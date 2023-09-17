using AnSumeri.API.Domain.Search.Filters;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Directors;

public interface IElasticSearchQueryDirector
{
    QueryContainer Direct(IArticleSearchFilter filter);
}
