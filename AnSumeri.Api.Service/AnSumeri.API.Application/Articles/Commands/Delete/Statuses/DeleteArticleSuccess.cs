namespace AnSumeri.API.Application.Articles.Commands.Delete.Statuses;

using OperationStatuses;

public class DeleteArticleSuccess : IOperationStatus
{
    private readonly int _id;

    public DeleteArticleSuccess(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.OK;
    public string Message => $"Article with id = {_id} was successfully deleted!";
}
