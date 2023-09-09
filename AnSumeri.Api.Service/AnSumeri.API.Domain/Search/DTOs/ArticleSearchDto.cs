namespace AnSumeri.API.Domain.Search;

public record ArticleSearchDto(int Id, Guid UserId, string Title, string Content);
