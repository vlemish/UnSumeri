using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Add
{
    public record AddCategoryResponse(IOperationStatus Status, AddCategoryResult Result);
}
