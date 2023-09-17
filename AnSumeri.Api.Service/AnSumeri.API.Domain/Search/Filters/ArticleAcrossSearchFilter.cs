using System.Linq.Expressions;
using AnSumeri.API.Domain.Search.Enums;

namespace AnSumeri.API.Domain.Search.Filters;

public record ArticleAcrossSearchFilter(string QueryBy, Guid UserId, SearchMode SearchMode = SearchMode.Across,
    params Expression<Func<ArticleSearchDto, string>>[] Properties) : IArticleSearchFilter;
