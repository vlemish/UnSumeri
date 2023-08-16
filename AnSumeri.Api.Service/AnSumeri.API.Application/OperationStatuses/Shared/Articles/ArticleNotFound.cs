namespace AnSumeri.API.Application.OperationStatuses.Shared.Articles;

public class ArticleNotFound : IOperationStatus
{
    private readonly int _articleId;

    public ArticleNotFound(int articleId)
    {
        this._articleId = articleId;
    }

    public OperationStatusValue Status => OperationStatusValue.NotFound;

    public string Message => $"Article with Id = {this._articleId} doesn't exist!";
}
