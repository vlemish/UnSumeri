using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Categories.Queries.FindMany.Statuses;

public class FindManyByFilterSuccess : IOperationStatus
{
    public OperationStatusValue Status => OperationStatusValue.OK;
    public string Message => "Categories were successfully found by specified filter";
}
