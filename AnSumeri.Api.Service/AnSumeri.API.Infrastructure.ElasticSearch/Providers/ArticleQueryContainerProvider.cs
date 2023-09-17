using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Enums;
using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Factories;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Providers;

public class ArticleQueryContainerProvider : IQueryContainerProvider<ArticleSearchDto, ArticleAllShouldMustFilter>
{
    private Dictionary<SearchMode,
            Func<QueryContainerDescriptor<ArticleSearchDto>, ArticleAllShouldMustFilter, QueryContainer>>
        _searchModeQueryContainerMappings;

    private readonly IElasticSearchQueryFactory<ArticleSearchDto, ArticleAllShouldMustFilter> _elasticSearchQueryFactory;

    public ArticleQueryContainerProvider(
        IElasticSearchQueryFactory<ArticleSearchDto, ArticleAllShouldMustFilter> elasticSearchQueryFactory)
    {
        _elasticSearchQueryFactory = elasticSearchQueryFactory;
        DefineMappings();
    }

    public QueryContainer Provide(QueryContainerDescriptor<ArticleSearchDto> descriptor, ArticleAllShouldMustFilter filter) =>
        _searchModeQueryContainerMappings.ContainsKey(filter.SearchMode)
            ? _searchModeQueryContainerMappings[filter.SearchMode](descriptor, filter)
            : throw new ArgumentOutOfRangeException($"{filter.SearchMode} not supported yet!");

    private void DefineMappings() =>
        _searchModeQueryContainerMappings = new()
        {
            { SearchMode.Across, _elasticSearchQueryFactory.CreateAllMatchQuery },
            { SearchMode.SingleProperty, _elasticSearchQueryFactory.CreateSingleMatchQuery },
            { SearchMode.AllTrue, _elasticSearchQueryFactory.CreateAllTrueQuery },
            { SearchMode.SomeTrue, _elasticSearchQueryFactory.CreateSomeTrueQuery },
        };
}
