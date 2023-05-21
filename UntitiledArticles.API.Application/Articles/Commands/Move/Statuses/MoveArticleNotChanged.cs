namespace UntitiledArticles.API.Application.Articles.Commands.Move.Statuses;

using OperationStatuses;

public class MoveArticleNotChanged : IOperationStatus
{
    private readonly int _id;
    private readonly int _categoryId;

    public MoveArticleNotChanged(int id, int categoryId)
    {
        _id = id;
        _categoryId = categoryId;
    }

    public OperationStatusValue Status => OperationStatusValue.NotModified;

    public string Message => $"Article wasn't moved, the same location was passed where id = {this._id} and categoryId = {this._categoryId}";
}
