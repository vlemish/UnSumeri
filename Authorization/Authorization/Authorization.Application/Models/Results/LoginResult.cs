namespace Authorization.Application.Models.Results;

using Authorization.Application.Models.Enumerations;

public class LoginResult : AuthenticationResult<AuthenticationErrorReason>
{
    public AuthenticationJwtToken AuthenticationJwtToken { get; }

    public LoginResult(bool success, AuthenticationJwtToken authenticationJwtToken = null, List<ErrorResult<AuthenticationErrorReason>> errorResult = null)
        : base(success, errorResult)
    {
        this.AuthenticationJwtToken = authenticationJwtToken;
    }
}
