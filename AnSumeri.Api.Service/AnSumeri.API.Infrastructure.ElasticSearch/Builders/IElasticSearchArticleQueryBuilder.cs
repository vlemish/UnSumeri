using System.Linq.Expressions;
using AnSumeri.API.Domain.Search;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Builders;

public interface IElasticSearchArticleQueryBuilder
{
    IElasticSearchArticleQueryBuilder AddBase(Guid userId);

    IElasticSearchArticleQueryBuilder AddMustMatch<TValue>(Expression<Func<ArticleSearchDto, TValue>> field,
        string queryBy);

    IElasticSearchArticleQueryBuilder AddShouldMatch<TValue>(Expression<Func<ArticleSearchDto, TValue>> field,
        string queryBy);

    IElasticSearchArticleQueryBuilder AddMultiMatchQuery<TValue>(
        string queryBy, params Expression<Func<ArticleSearchDto, TValue>>[] fields);

    QueryContainer Build();
}
