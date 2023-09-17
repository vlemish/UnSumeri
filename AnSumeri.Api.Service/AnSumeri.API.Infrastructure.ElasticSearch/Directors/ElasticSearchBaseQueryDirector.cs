using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Builders;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Directors;

public abstract class ElasticSearchBaseQueryDirector : IElasticSearchQueryDirector
{
    protected readonly IElasticSearchArticleQueryBuilder _queryBuilder;

    public ElasticSearchBaseQueryDirector(IElasticSearchArticleQueryBuilder queryBuilder)
    {
        _queryBuilder = queryBuilder;
    }

    public abstract QueryContainer Direct(IArticleSearchFilter filter);
}
