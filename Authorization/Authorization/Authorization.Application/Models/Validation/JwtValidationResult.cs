namespace Authorization.Application.Models.Validation;

using Authorization.Application.Abstractions.Validation;
using Authorization.Application.Models.Enumerations;
using Results;

public class JwtValidationResult : ValidationResult<JwtValidationErrorReason>
{
    public override bool Valid { get; protected set; }

    public override IEnumerable<ErrorResult<JwtValidationErrorReason>> ErrorResults { get; protected set; }

    public JwtValidationResult() : base()
    {
    }

    public JwtValidationResult(IEnumerable<ErrorResult<JwtValidationErrorReason>> errorResults) : base(errorResults)
    {
    }
}
