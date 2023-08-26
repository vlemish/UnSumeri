using AnSumeri.API.Domain.Contracts;

namespace AnSumeri.API.Application.Models.Providers;

public class UtcDateTimeProvider : IDateTimeProvider
{
    public DateTime Current { get; set; } = DateTime.UtcNow;
}
