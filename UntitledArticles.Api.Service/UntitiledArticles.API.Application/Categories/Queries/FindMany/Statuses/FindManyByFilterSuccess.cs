using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries.FindMany.Statuses;

public class FindManyByFilterSuccess : IOperationStatus
{
    public OperationStatusValue Status => OperationStatusValue.OK;
    public string Message => "Categories were successfully found by specified filter";
}
