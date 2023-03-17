using MediatR;

namespace UntitiledArticles.API.Application.Categories.Queries
{
    public record GetCategory(int Id) : IRequest<GetCategoryResponse>;
}
