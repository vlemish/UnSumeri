using AnSumeri.API.Domain.Search.Enums;

namespace AnSumeri.API.Domain.Search.Filters;

public interface IArticleSearchFilter
{
    Guid UserId { get; init; }

    SearchMode SearchMode { get; init; }
}
