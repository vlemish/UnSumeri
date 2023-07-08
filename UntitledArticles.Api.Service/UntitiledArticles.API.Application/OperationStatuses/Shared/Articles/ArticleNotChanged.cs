namespace UntitiledArticles.API.Application.OperationStatuses.Shared.Articles;

using UntitiledArticles.API.Application.OperationStatuses;

public class ArticleNotChanged : IOperationStatus
{
    private readonly int _id;

    public ArticleNotChanged(int id)
    {
        this._id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.NotModified;

    public string Message => $"Requested updated doesn't update any of passed value where Id = {_id}";
}
