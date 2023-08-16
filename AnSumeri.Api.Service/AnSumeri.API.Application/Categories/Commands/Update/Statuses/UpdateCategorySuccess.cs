using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Categories.Commands.Update.Statuses;
public class UpdateCategorySuccess : IOperationStatus
{
    private readonly int _id;

    public UpdateCategorySuccess(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.NoContent;

    public string Message => $"Category where id = {_id} was successfully updated!";
}
