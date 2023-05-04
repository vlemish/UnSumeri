namespace UntitiledArticles.API.Application.Categories.Queries.GetAll;

public class GetAllCategoriesResult
{
    public int Id { get; init; }

    public string Name { get; init; }

    public int? ParentId { get; init; }

    public IReadOnlyCollection<GetAllCategoriesResult> SubCategories { get; init; }
}