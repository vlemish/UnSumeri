using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    public record AddCategory(string Name) : IRequest<AddCategoryResponse>;
}
