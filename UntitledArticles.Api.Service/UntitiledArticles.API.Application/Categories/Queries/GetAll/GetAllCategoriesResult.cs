namespace UntitiledArticles.API.Application.Categories.Queries.GetAll;

using Models;
using UntitledArticles.API.Domain.Entities;

public class GetAllCategoriesResult
{
    public int Id { get; init; }

    public string Name { get; init; }

    public int? ParentId { get; init; }

    public IReadOnlyCollection<GetAllCategoriesResult> SubCategories { get; init; }

    public IReadOnlyCollection<ArticleDto> Articles { get; init; }
}
