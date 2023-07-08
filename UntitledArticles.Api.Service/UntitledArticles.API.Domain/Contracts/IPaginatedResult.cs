namespace UntitledArticles.API.Domain.Contracts;

public interface IPaginatedResult<T>
{
    IReadOnlyCollection<T> Records { get; }
    
    int TotalRecordsCount { get; }
}