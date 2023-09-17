using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Enums;
using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Builders;
using AnSumeri.API.Infrastructure.ElasticSearch.Directors;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Factories;

public class ElasticSearchQueryDirectorFactory : IElasticSearchQueryDirectorFactory
{
    public IElasticSearchQueryDirector CreateQueryDirector(SearchMode searchMode,
        QueryContainerDescriptor<ArticleSearchDto> queryContainerDescriptor)
    {
        IElasticSearchArticleQueryBuilder articleQueryBuilder =
            new ElasticSearchArticleQueryBuilder(queryContainerDescriptor);

        IElasticSearchQueryDirector director;
        switch (searchMode)
        {
            case SearchMode.Across:
                director = new ElasticSearchFullTextSearchQueryDirector(articleQueryBuilder);
                break;
            case SearchMode.AllTrue:
                director = new ElasticSearchAllMatchQueryDirector(articleQueryBuilder);
                break;
            case SearchMode.SomeTrue:
                director = new ElasticSearchAllShouldMatchQueryDirector(articleQueryBuilder);
                break;
            case SearchMode.SingleProperty:
                director =  new ElasticSearchMustMatchQueryDirector(articleQueryBuilder);
                break;
            default: throw new ArgumentOutOfRangeException($"{searchMode} not supported");
        }

        return director;
    }
}
