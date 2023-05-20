using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete;

using Models.Mediatr;

public record DeleteCategory(int Id) : IRequest<ResultDto>;
