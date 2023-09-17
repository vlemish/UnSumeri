using AnSumeri.API.Domain.Search;
using AnSumeri.API.Domain.Search.Enums;
using AnSumeri.API.Domain.Search.Filters;
using AnSumeri.API.Infrastructure.ElasticSearch.Directors;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Factories;

public interface IElasticSearchQueryDirectorFactory
{
    IElasticSearchQueryDirector CreateQueryDirector(SearchMode searchMode,
        QueryContainerDescriptor<ArticleSearchDto> queryContainerDescriptor);
}
