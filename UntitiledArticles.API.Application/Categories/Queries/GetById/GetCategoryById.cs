using MediatR;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    public record GetCategoryById(int Id) : IRequest<GetCategoryByIdResponse>;
}
