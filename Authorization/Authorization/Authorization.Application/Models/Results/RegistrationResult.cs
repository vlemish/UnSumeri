namespace Authorization.Application.Models.Results;

using Authorization.Application.Models.Enumerations;

public class RegistrationResult : AuthenticationResult<AuthenticationErrorReason>
{
    public RegistrationResult(bool success, List<ErrorResult<AuthenticationErrorReason>> errorResult = null) : base(success, errorResult)
    {
    }
}
