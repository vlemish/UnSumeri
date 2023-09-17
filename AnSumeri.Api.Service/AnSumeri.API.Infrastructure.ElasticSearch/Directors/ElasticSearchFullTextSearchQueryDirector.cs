using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Builders;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Directors;

public class ElasticSearchFullTextSearchQueryDirector : ElasticSearchBaseQueryDirector
{
    public ElasticSearchFullTextSearchQueryDirector(IElasticSearchArticleQueryBuilder queryBuilder) : base(queryBuilder)
    {
    }

    public override QueryContainer Direct(IArticleSearchFilter filter)
    {
        ArticleAcrossSearchFilter articleAcrossSearchFilter = filter as ArticleAcrossSearchFilter;
        return  _queryBuilder.AddBase(filter.UserId)
            .AddMultiMatchQuery(articleAcrossSearchFilter.QueryBy, articleAcrossSearchFilter.Properties)
            .Build();
    }

}
