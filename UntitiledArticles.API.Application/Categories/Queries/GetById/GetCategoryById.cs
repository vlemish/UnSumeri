using MediatR;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    public record GetCategoryById(int Id, int Depth = 2) : IRequest<GetCategoryByIdResponse>;
}
