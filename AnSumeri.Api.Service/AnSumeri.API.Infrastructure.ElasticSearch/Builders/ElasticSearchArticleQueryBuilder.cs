using System.Linq.Expressions;
using AnSumeri.API.Domain.Search;
using Nest;

namespace AnSumeri.API.Infrastructure.ElasticSearch.Builders;

public class ElasticSearchArticleQueryBuilder : IElasticSearchArticleQueryBuilder
{
    private QueryContainerDescriptor<ArticleSearchDto> _queryContainerDescriptor;
    private BoolQueryDescriptor<ArticleSearchDto> articleSearchDto;

    public ElasticSearchArticleQueryBuilder(QueryContainerDescriptor<ArticleSearchDto> queryContainerDescriptor)
    {
        _queryContainerDescriptor = queryContainerDescriptor;
        _queryContainerDescriptor.Bool(b => articleSearchDto = b);
    }

    public IElasticSearchArticleQueryBuilder AddBase(Guid userId)
    {
        articleSearchDto.Must(m => m.Match(m => m
            .Field(f => f.UserId)
            .Query(userId.ToString())));
        return this;
    }

    public IElasticSearchArticleQueryBuilder AddMustMatch<TValue>(Expression<Func<ArticleSearchDto, TValue>> field,
        string queryBy)
    {
        articleSearchDto.Must(mu =>
            mu.Match(m =>
                m.Field(field).Query(queryBy)));
        return this;
    }

    public IElasticSearchArticleQueryBuilder AddShouldMatch<TValue>(Expression<Func<ArticleSearchDto, TValue>> field,
        string queryBy)
    {
        articleSearchDto.Should(mu => mu.Match(m => m.Field(field).Query(queryBy)));
        return this;
    }

    public IElasticSearchArticleQueryBuilder AddMultiMatchQuery<TValue>(
        string queryBy, params Expression<Func<ArticleSearchDto, TValue>>[] fields)
    {
        articleSearchDto.Must(
            // 2. Full-text search on Title and Content
            fs => fs
                .MultiMatch(m => m
                    .Fields(f => AddFieldsDescriptor(f, fields)
                    )
                    .Query(queryBy)
                ));

        return this;
    }

    private FieldsDescriptor<ArticleSearchDto> AddFieldsDescriptor<TValue>(FieldsDescriptor<ArticleSearchDto> fieldsDescriptor,
        Expression<Func<ArticleSearchDto, TValue>>[] fields)
    {
        foreach (var field in fields)
        {
            fieldsDescriptor.Field(field);
        }

        return fieldsDescriptor;
    }

    public QueryContainer Build()
    {
        return _queryContainerDescriptor;
    }
}
