namespace Authorization.Application.Models.Results;

public class ErrorResult<TReason> where TReason : Enum
{
    public TReason Reason { get; }

    public string Description { get; }

    public ErrorResult(TReason reason, string description)
    {
        this.Reason = reason;
        this.Description = description;
    }
}
