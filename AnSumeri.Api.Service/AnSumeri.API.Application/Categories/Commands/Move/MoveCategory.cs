using MediatR;

namespace AnSumeri.API.Application.Categories.Commands.Move
{
    using Models.Mediatr;

    public record MoveCategory(int Id, string UserId, int? MoveToId) : CategoryBaseRequest<ResultDto>(UserId);
}
