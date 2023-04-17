using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete.Statuses;

public class DeleteCategoryNotFoundStatus : IOperationStatus
{
    private readonly int _id;

    public DeleteCategoryNotFoundStatus(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status { get; } = OperationStatusValue.NotFound;
    public string Message { get; } = $"Category when Category Id = {_id} doesn't exist!";
}