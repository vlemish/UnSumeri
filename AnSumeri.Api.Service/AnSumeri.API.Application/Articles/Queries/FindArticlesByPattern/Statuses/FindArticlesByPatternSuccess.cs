using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern.Statuses;

public class FindArticlesByPatternSuccess : IOperationStatus
{
    private readonly string _userId;
    private readonly string _searchPattern;

    public FindArticlesByPatternSuccess(string userId, string searchPattern)
    {
        _userId = userId;
        _searchPattern = searchPattern;
    }

    public OperationStatusValue Status => OperationStatusValue.OK;

    public string Message => $"Articles with Search Pattern = {_searchPattern} for User Id = {_userId}";
}
