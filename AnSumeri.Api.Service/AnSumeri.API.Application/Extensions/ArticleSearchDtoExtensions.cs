using AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern;
using AnSumeri.API.Domain.Search;

namespace AnSumeri.API.Application.Extensions;

internal static class ArticleSearchDtoExtensions
{
    internal static FindArticlesByPatternResult ToFindArticleByPatternResult(this ArticleSearchDto articleSearchDto) =>
        new(articleSearchDto.Id, articleSearchDto.Title, articleSearchDto.Content);
}
