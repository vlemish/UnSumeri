namespace Authorization.Application.Models.Validation;

using Authorization.Application.Abstractions.Validation;
using Authorization.Application.Models.Enumerations;
using Results;

public class AuthenticationValidationResult : ValidationResult<AuthenticationErrorReason>
{
    public override bool Valid { get; protected set; }

    public override IEnumerable<ErrorResult<AuthenticationErrorReason>> ErrorResults { get; protected set; }

    public AuthenticationValidationResult() : base()
    {
    }

    public AuthenticationValidationResult(IEnumerable<ErrorResult<AuthenticationErrorReason>> errorResults) : base(errorResults)
    {
    }
}
