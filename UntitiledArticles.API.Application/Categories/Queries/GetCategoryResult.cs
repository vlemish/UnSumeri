namespace UntitiledArticles.API.Application.Categories.Queries
{
    public record GetCategoryResult
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int? ParentId { get; init; }

        public IReadOnlyCollection<GetCategoryResult> SubCategories { get; init; }
    }
}
