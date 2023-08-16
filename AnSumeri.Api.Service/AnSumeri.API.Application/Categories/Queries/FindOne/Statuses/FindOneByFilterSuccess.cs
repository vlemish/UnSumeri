namespace AnSumeri.API.Application.Categories.Queries.FindOne.Statuses;

using OperationStatuses;

public class FindOneByFilterSuccess : IOperationStatus
{
    public OperationStatusValue Status => OperationStatusValue.OK;
    public string Message => "The record was found by specified filter";
}
