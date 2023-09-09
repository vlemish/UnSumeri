using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Providers;

public interface IQueryContainerProvider<TResult, TFilter> where TResult: class
{
    QueryContainer Provide(QueryContainerDescriptor<TResult> descriptor,
        TFilter filter);
}
