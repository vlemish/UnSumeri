using AnSumeri.API.Domain.Search;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Factories;

public class ElasticSearchArticleQueryFactory : IElasticSearchQueryFactory<ArticleSearchDto, ArticleSearchFilter>
{
    #region IElasticSearchQueryFactory Implementation

    public QueryContainer CreateAllMatchQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b => b
            .Must(
                // 1. Match the UserId
                mu => CreateUserIdQuery(mu, filter),
                // 2. Full-text search on Title and Content
                fs => fs
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field(f1 => f1.Title)
                            .Field(f2 => f2.Content)
                        )
                        .Query(filter.Content)
                    )
            )
        );

    public QueryContainer CreateSingleMatchQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter)
    {
        return descriptor.Bool(b => b
            .Must(
                // 1. Match the UserId
                mu => CreateUserIdQuery(mu, filter),
                // 2. Search by Content
                fs => CreateSingleMatchSubQuery(fs, filter)));
    }

    public QueryContainer CreateAllTrueQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b => b.
            Must(
                // 1. Match the UserId
                mu => CreateUserIdQuery(mu, filter))
            .Must(
                m => m
                    .Match(condition => condition.
                        Field(f => f.Content)
                        .Query(filter.Content)))
                .Must(m => m
                    .Match(condition => condition
                        .Field(f => f.Title)
                        .Query(filter.Title))));

    public QueryContainer CreateSomeTrueQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b => b
            .Must(
                // 1. Match the UserId
                mu => CreateUserIdQuery(mu, filter))
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

    #endregion

    #region Private members

    private QueryContainer CreateUserIdQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Match(m => m
            .Field(f => f.UserId)
            .Query(filter.UserId.ToString()));

    private QueryContainer CreateSingleMatchSubQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        String.IsNullOrEmpty(filter.Title)
            ? descriptor
                .Match(q =>
                    q.Field(f1 => f1.Content).Query(filter.Content))
            : descriptor
                .Match(q =>
                    q.Field(f1 => f1.Title).Query(filter.Title));

    #endregion

}
