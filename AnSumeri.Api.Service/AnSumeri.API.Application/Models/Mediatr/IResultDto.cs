namespace AnSumeri.API.Application.Models.Mediatr;

public interface IResultDto<TOperationStatusSource>
{
    TOperationStatusSource OperationStatus { get; }

    string Message { get; }
}

public interface IResultDto<TOperationStatusSource, TPayload>
{
    TOperationStatusSource OperationStatus { get; }

    string Message { get; }

    TPayload Payload { get; }
}
