using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Enums;
using AnSumeri.API.Infrastructure.ElasticSearch.Factories;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Providers;

public class ArticleQueryContainerProvider : IQueryContainerProvider<ArticleSearchDto, ArticleSearchFilter>
{
    private Dictionary<SearchMode,
            Func<QueryContainerDescriptor<ArticleSearchDto>, ArticleSearchFilter, QueryContainer>>
        _searchModeQueryContainerMappings;

    private readonly IElasticSearchQueryFactory<ArticleSearchDto, ArticleSearchFilter> _elasticSearchQueryFactory;

    public ArticleQueryContainerProvider(
        IElasticSearchQueryFactory<ArticleSearchDto, ArticleSearchFilter> elasticSearchQueryFactory)
    {
        _elasticSearchQueryFactory = elasticSearchQueryFactory;
        DefineMappings();
    }

    public QueryContainer Provide(QueryContainerDescriptor<ArticleSearchDto> descriptor, ArticleSearchFilter filter) =>
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
