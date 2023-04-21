namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    public record GetCategoryByIdResult
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int? ParentId { get; init; }

        public IReadOnlyCollection<GetCategoryByIdResult> SubCategories { get; init; }
    }
}
