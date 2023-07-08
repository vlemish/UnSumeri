namespace Authorization.Application.Models;

using Enumerations;
using Results;

public abstract class AuthenticationResult<T> where T: Enum
{
    public bool Success { get; protected set; }

    public List<ErrorResult<T>> ErrorResult { get; protected set; }

    public AuthenticationResult(bool success)
    {
        Success = success;
        ErrorResult = new();
    }

    public AuthenticationResult(bool success, List<ErrorResult<T>> errorResult)
    {
        Success = success;
        ErrorResult = errorResult;
    }
}
