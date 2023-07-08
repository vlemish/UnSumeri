namespace Authorization.Application.Abstractions.Validation;

using Models.Enumerations;
using Models.Results;

public abstract class ValidationResult<TReason> : IValidationResult<TReason> where TReason : Enum
{
    abstract public bool Valid { get; protected set; }

    abstract public IEnumerable<ErrorResult<TReason>> ErrorResults { get; protected set; }

    public ValidationResult()
    {
        this.Valid = true;
        this.ErrorResults = new List<ErrorResult<TReason>>();
    }

    public ValidationResult(IEnumerable<ErrorResult<TReason>> errorResults)
    {
        this.Valid = false;
        this.ErrorResults = errorResults;
    }
}
