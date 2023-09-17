using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Builders;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Directors;

public class ElasticSearchMustMatchQueryDirector : ElasticSearchBaseQueryDirector
{
    public ElasticSearchMustMatchQueryDirector(IElasticSearchArticleQueryBuilder queryBuilder) : base(queryBuilder)
    {
    }

    public override QueryContainer Direct(IArticleSearchFilter filter)
    {
        ArticleSinglePropertyFilter articleSinglePropertyFilter = filter as ArticleSinglePropertyFilter;
        return _queryBuilder.AddBase(filter.UserId)
            .AddMustMatch(articleSinglePropertyFilter.Property, articleSinglePropertyFilter.QueryBy)
            .Build();
    }
}
