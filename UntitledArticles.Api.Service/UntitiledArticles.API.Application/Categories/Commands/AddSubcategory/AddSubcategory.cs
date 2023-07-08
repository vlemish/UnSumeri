using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    using Models.Mediatr;

    public record AddSubcategory
        (string Name, string UserId, int ParentId) : CategoryBaseRequest<ResultDto<AddSubcategoryResult>>(UserId);
}
