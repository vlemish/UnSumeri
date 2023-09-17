using System.Linq.Expressions;
using AnSumeri.API.Domain.Search.Enums;

namespace AnSumeri.API.Domain.Search.Filters;

public record ArticleSinglePropertyFilter(Guid UserId, Expression<Func<ArticleSearchDto, string>> Property,
    string QueryBy, SearchMode SearchMode = SearchMode.SingleProperty) : IArticleSearchFilter;
