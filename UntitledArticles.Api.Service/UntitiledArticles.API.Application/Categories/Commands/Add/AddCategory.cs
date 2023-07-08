using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    using Models.Mediatr;

    public record AddCategory(string Name, string UserId) : CategoryBaseRequest<ResultDto<AddCategoryResult>>(UserId);
}
