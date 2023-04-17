using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete;

public record DeleteCategory(int Id) : IRequest<DeleteCategoryResponse>;