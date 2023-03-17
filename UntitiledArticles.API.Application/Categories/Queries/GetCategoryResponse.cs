using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries
{
    public record GetCategoryResponse(IOperationStatus Status, GetCategoryResult Result);
}
