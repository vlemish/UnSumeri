using AnSumeri.API.Domain.Search;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Factories;

public class ElasticSearchArticleQueryFactory : IElasticSearchQueryFactory<ArticleSearchDto, ArticleSearchFilter>
{
    public QueryContainer CreateAllMatchQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.MultiMatch(q =>
            q.Fields(f => f.Field(f1 => f1.Content).Field(f2 => f2.Title)).Query(filter.Title));

    public QueryContainer CreateSingleMatchQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter)
    {
        if (String.IsNullOrEmpty(filter.Title))
        {
            return descriptor.Match(q =>
                q.Field(f1 => f1.Content).Query(filter.Content));
        }

        return descriptor.Match(q =>
            q.Field(f1 => f1.Title).Query(filter.Title));
    }

    public QueryContainer CreateAllTrueQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b =>
            b.Must(m => m.Match(condition => condition.Field(f => f.Content).Query(filter.Content))).Must(m =>
                m.Match(condition => condition.Field(f => f.Title).Query(filter.Title))));

    public QueryContainer CreateSomeTrueQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b => b
            .Should(
                sh => sh.Match(m => m
                    .Field(f => f.Content)
                    .Query(filter.Content)
                ),
                sh => sh.Match(m => m
                    .Field(f => f.Title)
                    .Query(filter.Title)
                )
            )
        );
}
