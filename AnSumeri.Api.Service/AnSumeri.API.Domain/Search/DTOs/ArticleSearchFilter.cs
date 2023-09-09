using AnSumeri.API.Domain.Search.Enums;

namespace AnSumeri.API.Domain.Search;

public record ArticleSearchFilter(Guid UserId, string? Title, string? Content, SearchMode SearchMode);
