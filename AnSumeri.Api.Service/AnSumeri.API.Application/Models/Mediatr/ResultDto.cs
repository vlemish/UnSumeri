namespace AnSumeri.API.Application.Models.Mediatr;

using OperationStatuses;

public class ResultDto : IResultDto<IOperationStatus>
{
    public ResultDto(IOperationStatus status)
    {
        OperationStatus = status;
    }

    public ResultDto(IOperationStatus status, string message)
    {
        OperationStatus = status;
        Message = message;
    }

    public IOperationStatus OperationStatus { get; }

    public string Message { get; }
}

public class ResultDto<TPayload> : IResultDto<IOperationStatus, TPayload>
{
    public ResultDto(IOperationStatus status, TPayload payload)
    {
        OperationStatus = status;
        Payload = payload;
    }

    public ResultDto(IOperationStatus status, TPayload payload, string message)
    {
        OperationStatus = status;
        Payload = payload;
        Message = message;
    }

    public IOperationStatus OperationStatus { get; }

    public string Message { get; }

    public TPayload Payload { get; }
}
