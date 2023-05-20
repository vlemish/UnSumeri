namespace UntitiledArticles.API.Application.Articles.Queries.GetOneById.Statuses;

using OperationStatuses;

public class GetOneByIdArticleNotFound : IOperationStatus
{
    private readonly int _id;

    public GetOneByIdArticleNotFound(int id)
    {
        _id = id;
    }

    public OperationStatusValue Status => OperationStatusValue.NotFound;
    public string Message => $"Couldn't get article by id = {_id}";
}
