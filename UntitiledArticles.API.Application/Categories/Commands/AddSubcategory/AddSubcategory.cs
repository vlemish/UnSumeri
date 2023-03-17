using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    public record AddSubcategory(string Name, int ParentId) : IRequest<AddSubcategoryResponse>;
}
