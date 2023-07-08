namespace UntitiledArticles.API.Application.Articles.Queries.GetOneById.Statuses;

using OperationStatuses;

public class GetOneArticleByIdSuccess : IOperationStatus
{
    private readonly int _id;

    public GetOneArticleByIdSuccess(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.OK;

    public string Message => $"Article where id = {_id} was successfully found!";
}
