using MediatR;

namespace AnSumeri.API.Application.Categories.Commands.MoveAsRoot;

using Models.Mediatr;

public record MoveAsRoot(int Id, string UserId) : CategoryBaseRequest<ResultDto>(UserId);
