using AnSumeri.API.Domain.Search;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Factories;

public class ElasticSearchArticleQueryFactory : IElasticSearchQueryFactory<ArticleSearchDto, ArticleSearchFilter>
{
    public QueryContainer CreateAllMatchQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b => b
            .Must(
                // 1. Match the UserId
                mu => mu
                    .Match(m => m
                        .Field(f => f.UserId)
                        .Query(filter.UserId.ToString())
                    ),
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
        if (String.IsNullOrEmpty(filter.Title))
        {
            return descriptor.Bool(b => b
                .Must(
                    // 1. Match the UserId
                    mu => mu
                        .Match(m => m
                            .Field(f => f.UserId)
                            .Query(filter.UserId.ToString())
                        ),
                    // 2. Search by Content
                    fs => fs
                        .Match(q =>
                            q.Field(f1 => f1.Content).Query(filter.Content))
                )
            );
        }

        return descriptor.Bool(b => b
            .Must(
                // 1. Match the UserId
                mu => mu
                    .Match(m => m
                        .Field(f => f.UserId)
                        .Query(filter.UserId.ToString())
                    ),
                // 2. Search by Content
                fs => fs
                    .Match(q =>
                        q.Field(f1 => f1.Content).Query(filter.Content))));
    }

    public QueryContainer CreateAllTrueQuery(QueryContainerDescriptor<ArticleSearchDto> descriptor,
        ArticleSearchFilter filter) =>
        descriptor.Bool(b => b.
            Must(
                // 1. Match the UserId
                mu => mu
                    .Match(m => m
                        .Field(f => f.UserId)
                        .Query(filter.UserId.ToString())))
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
                mu => mu
                    .Match(m => m
                        .Field(f => f.UserId)
                        .Query(filter.UserId.ToString())))
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
