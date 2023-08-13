using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Move.Statuses;

public class MoveCategoryNotModified : IOperationStatus
{
    private readonly int _id;
    private readonly int? _parentId;

    public MoveCategoryNotModified(int id, int? parentId)
    {
        _id = id;
        _parentId = parentId;
    }

    public OperationStatusValue Status => OperationStatusValue.NotModified;
    public string Message => $"Category wasn't moved where Id = {_id} and parentId = {_parentId}";
}
