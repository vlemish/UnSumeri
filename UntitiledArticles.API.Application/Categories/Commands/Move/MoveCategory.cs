using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Move
{
    public record MoveCategory(int Id, int? ParentId) : IRequest<MoveCategoryResponse>;
}
