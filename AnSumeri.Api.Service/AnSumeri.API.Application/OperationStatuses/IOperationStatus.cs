namespace AnSumeri.API.Application.OperationStatuses
{
    public interface IOperationStatus
    {
        OperationStatusValue Status { get; }

        string Message { get; }
    }
}
