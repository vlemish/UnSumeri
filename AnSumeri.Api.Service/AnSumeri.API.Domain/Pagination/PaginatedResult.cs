using AnSumeri.API.Domain.Contracts;

namespace AnSumeri.API.Domain.Pagination;

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