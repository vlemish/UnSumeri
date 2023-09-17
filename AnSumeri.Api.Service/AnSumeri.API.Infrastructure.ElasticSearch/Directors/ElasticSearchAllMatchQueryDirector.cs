using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Builders;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Directors;

public class ElasticSearchAllMatchQueryDirector : ElasticSearchBaseQueryDirector
{
    public ElasticSearchAllMatchQueryDirector(IElasticSearchArticleQueryBuilder queryBuilder) : base(queryBuilder)
    {
    }

    public override QueryContainer Direct(IArticleSearchFilter filter)
    {
        ArticleAllShouldMustFilter? articleAllShouldMustFilter = filter as ArticleAllShouldMustFilter;
        return _queryBuilder.AddBase(filter.UserId)
            .AddMustMatch(x => x.Title, articleAllShouldMustFilter.Title)
            .AddMustMatch(x => x.Content, articleAllShouldMustFilter.Content)
            .Build();
    }
}
