namespace UntitiledArticles.API.Application.OperationStatuses
{
    public interface IOperationStatus
    {
        OperationStatusValue Status { get; }

        string Message { get; }
    }
}
