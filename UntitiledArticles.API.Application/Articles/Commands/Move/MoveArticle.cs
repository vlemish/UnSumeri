namespace UntitiledArticles.API.Application.Articles.Commands.Move;

using MediatR;
using Models.Mediatr;

public record MoveArticle(int Id, int CategoryToMoveId) : IRequest<ResultDto>;
