namespace Authorization.Application.Abstractions.Validation;

using Models.Results;

public interface IValidationResult<TReason> where TReason: Enum
{
    public bool Valid { get; }

    public IEnumerable<ErrorResult<TReason>> ErrorResults { get; }
}
