using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Delete.Statuses;

public class DeleteCategorySuccessStatus : IOperationStatus
{
    private readonly int _id;
    
    public DeleteCategorySuccessStatus(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status { get; } = OperationStatusValue.OK;
    public string Message { get; } = $"Category when Category Id = {_id} was successfully deleted!";
}