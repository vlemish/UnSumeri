namespace UntitiledArticles.API.Application.Articles.Commands.Move.Statuses;

using Models.Mediatr;
using OperationStatuses;

public class MoveArticleSuccess : IOperationStatus
{
    private readonly int _id;
    private readonly int _categoryId;

    public MoveArticleSuccess(int id, int categoryId)
    {
        _id = id;
        _categoryId = categoryId;
    }

    public OperationStatusValue Status => OperationStatusValue.NoContent;

    public string Message => $"Article was successfully moved when id = {this._id} and categoryId = {this._categoryId}";
}
