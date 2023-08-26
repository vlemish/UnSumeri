namespace AnSumeri.API.Domain.Contracts;

public interface IDateTimeProvider
{
    public DateTime Current { get; set; }
}
