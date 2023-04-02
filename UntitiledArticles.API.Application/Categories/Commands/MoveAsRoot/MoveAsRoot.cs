using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;

public record MoveAsRoot(int Id) : IRequest<MoveAsRootResponse>;