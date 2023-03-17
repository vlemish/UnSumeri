namespace UntitiledArticles.API.Application.Models.Categories
{
    public record CategoryReadDto
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int? ParentId { get; init; }

        public IReadOnlyCollection<CategoryReadDto> SubCategories { get; init; }
    }
}
