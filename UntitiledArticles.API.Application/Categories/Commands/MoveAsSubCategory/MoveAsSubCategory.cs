using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;

public record MoveAsSubCategory(int Id, int MoveToId) : IRequest<MoveAsSubCategoryResponse>;