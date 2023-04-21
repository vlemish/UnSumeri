using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete.Statuses;

public class DeleteCategoryNotFound : IOperationStatus
{
    private readonly int _id;

    public DeleteCategoryNotFound(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status { get; } = OperationStatusValue.NotFound;
    public string Message => $"Category when Category Id = {_id} doesn't exist!";
}