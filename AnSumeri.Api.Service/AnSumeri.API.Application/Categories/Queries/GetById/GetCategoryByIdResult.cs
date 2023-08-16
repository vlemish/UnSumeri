using AnSumeri.API.Application.Models;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Categories.Queries.GetById
{
    public record GetCategoryByIdResult
    {
        public int Id { get; init; }

        public string UserId { get; init; }

        public string Name { get; init; }

        public int? ParentId { get; init; }

        public IReadOnlyCollection<ArticleDto> Articles { get; set; }

        public IReadOnlyCollection<GetCategoryByIdResult> SubCategories { get; init; }
    }
}
