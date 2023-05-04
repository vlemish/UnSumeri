using UntitledArticles.API.Domain.Contracts;

namespace UntitledArticles.API.Domain.Pagination;

public class PaginatedResult<T> : IPaginatedResult<T>
{
    public PaginatedResult(IList<T> records, int totalRecords)
    {
        Records = records.ToList().AsReadOnly();
        TotalRecordsCount = totalRecords;
    }
    
    public IReadOnlyCollection<T> Records { get; }
    public int TotalRecordsCount { get; }
}