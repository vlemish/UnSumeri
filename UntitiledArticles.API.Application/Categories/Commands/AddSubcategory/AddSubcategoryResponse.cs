using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.AddSubcategory
{
    public record AddSubcategoryResponse(IOperationStatus Status, AddSubcategoryResult Result);
}
