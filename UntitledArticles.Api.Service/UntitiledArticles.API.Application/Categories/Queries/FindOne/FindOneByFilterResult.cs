namespace UntitiledArticles.API.Application.Categories.Queries.FindOne;

using GetById;
using Models;

public class FindOneByFilterResult
{
    public int Id { get; init; }

    public string UserId { get; init; }

    public string Name { get; init; }

    public int? ParentId { get; init; }

    public IReadOnlyCollection<ArticleDto> Articles { get; set; }

    public IReadOnlyCollection<FindOneByFilterResult> SubCategories { get; init; }
}
