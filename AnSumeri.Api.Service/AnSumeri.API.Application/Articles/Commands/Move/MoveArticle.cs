namespace AnSumeri.API.Application.Articles.Commands.Move;

using MediatR;
using Models.Mediatr;

public record MoveArticle(int Id, string UserId, int CategoryToMoveId) : IRequest<ResultDto>;
