using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Articles.Commands.Update.Statuses;

using Application.OperationStatuses;

public class UpdateArticleSuccess : IOperationStatus
{
    private readonly int _id;

    public UpdateArticleSuccess(int id)
    {
        this._id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.NoContent;

    public string Message => $"Article was successfully updated where id = {this._id}!";
}
