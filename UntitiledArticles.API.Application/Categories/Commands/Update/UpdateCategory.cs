using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Update;

public record UpdateCategory(int Id, string Name) : IRequest<UpdateCategoryResponse>;