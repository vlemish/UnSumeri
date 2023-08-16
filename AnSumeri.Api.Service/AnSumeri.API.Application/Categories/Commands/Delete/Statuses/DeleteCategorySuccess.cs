using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Categories.Commands.Delete.Statuses;

public class DeleteCategorySuccess : IOperationStatus
{
    private readonly int _id;

    public DeleteCategorySuccess(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.OK;
    public string Message => $"Category when Category Id = {_id} was successfully deleted!";
}
