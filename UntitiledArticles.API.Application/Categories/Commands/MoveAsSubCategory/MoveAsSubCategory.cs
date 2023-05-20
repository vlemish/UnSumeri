using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;

using Models.Mediatr;

public record MoveAsSubCategory(int Id, int MoveToId) : IRequest<ResultDto>;
