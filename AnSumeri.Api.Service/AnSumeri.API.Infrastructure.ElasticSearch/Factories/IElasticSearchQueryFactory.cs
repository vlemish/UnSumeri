using AnSumeri.API.Domain.Search;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Factories;

[Obsolete]
public interface IElasticSearchQueryFactory<TResult, TFilter> where TResult : class
{
    QueryContainer CreateAllMatchQuery(QueryContainerDescriptor<TResult> descriptor,
        TFilter filter);

    QueryContainer CreateSingleMatchQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        TFilter filter);

    QueryContainer CreateAllTrueQuery(QueryContainerDescriptor<TResult> descriptor,
        TFilter filter);

    QueryContainer CreateSomeTrueQuery(QueryContainerDescriptor<TResult> descriptor,
        TFilter filter);
}
