namespace Authorization.Application.Models.Results;

using Authorization.Application.Models.Enumerations;

public class RefreshTokenResult : AuthenticationResult<JwtValidationErrorReason>
{
    public AuthenticationJwtToken AuthenticationJwtToken { get; }

    public RefreshTokenResult(bool success)
        : base(success)
    {
        this.AuthenticationJwtToken = null;
    }

    public RefreshTokenResult(bool success, AuthenticationJwtToken authenticationJwtToken)
        : base(success)
    {
        this.AuthenticationJwtToken = authenticationJwtToken;
    }

    public RefreshTokenResult(bool success, AuthenticationJwtToken authenticationJwtToken, List<ErrorResult<JwtValidationErrorReason>> errorResult)
        : base(success, errorResult)
    {
        this.AuthenticationJwtToken = authenticationJwtToken;
    }
}
