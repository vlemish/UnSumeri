using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries.GetAll.Statuses;

public class GetAllCategoriesSuccess : IOperationStatus
{
    public OperationStatusValue Status { get; } = OperationStatusValue.OK;
    public string Message { get; } = $"Getting all categories was successfully performed!";
}