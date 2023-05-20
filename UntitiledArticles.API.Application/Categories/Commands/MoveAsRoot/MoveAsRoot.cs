using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;

using Models.Mediatr;

public record MoveAsRoot(int Id) : IRequest<ResultDto>;
