using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    public record GetCategoryByIdResponse(IOperationStatus Status, GetCategoryByIdResult Result);
}
