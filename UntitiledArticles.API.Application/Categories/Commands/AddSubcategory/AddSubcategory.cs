using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    using Models.Mediatr;

    public record AddSubcategory(string Name, int ParentId) : IRequest<ResultDto<AddSubcategoryResult>>;
}
