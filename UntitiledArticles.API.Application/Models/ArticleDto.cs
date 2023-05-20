namespace UntitiledArticles.API.Application.Models;

public record ArticleDto(int Id, string Title, string Content, DateTime CreatedAtTime, int CategoryId);
