using UntitledSelfArticles.API.Application.Enums;

namespace UntitledSelfArticles.API.Application.OperationsStatus
{
    public interface IOperationStatus
    {
        OperationsStatusValue Status { get; }

        string Message { get; set; }
    }
}
