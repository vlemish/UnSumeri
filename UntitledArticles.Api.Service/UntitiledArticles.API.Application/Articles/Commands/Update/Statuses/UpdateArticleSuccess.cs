namespace UntitiledArticles.API.Application.Articles.Commands.Update.Statuses;

using UntitiledArticles.API.Application.OperationStatuses;

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
