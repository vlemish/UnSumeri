namespace AnSumeri.API.Domain.Pagination;

public class LoadOptions
{
    private const int DefaultOffset = 25;
    private const int DefaultSkip = 0;
    
    public LoadOptions(int? offset = null, int? skip = null)
    {
        Offset = offset ?? DefaultOffset;
        Skip = skip ?? DefaultSkip;
    }
    
    public int Offset { get; } = 25;

    public int Skip { get; } = 0;
}