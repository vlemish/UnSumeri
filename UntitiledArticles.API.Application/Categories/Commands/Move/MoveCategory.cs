using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Move
{
    using Models.Mediatr;

    public record MoveCategory(int Id, int? MoveToId) : IRequest<ResultDto>;
}
