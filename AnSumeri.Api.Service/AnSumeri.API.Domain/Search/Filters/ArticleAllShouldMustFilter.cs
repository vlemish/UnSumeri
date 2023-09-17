using AnSumeri.API.Domain.Search.Enums;

namespace AnSumeri.API.Domain.Search.Filters;

public record ArticleAllShouldMustFilter
    (Guid UserId, string? Title, string? Content) : IArticleSearchFilter
{
    private SearchMode _searchMode;

    public SearchMode SearchMode
    {
        get => _searchMode;
        init
        {
            if (value != SearchMode.AllTrue || value != SearchMode.SomeTrue)
            {
                throw new ArgumentOutOfRangeException(
                    "This filter can be used only with SearchMode.AllTrue or SearchMode.SomeTrue");
            }

            _searchMode = SearchMode;
        }
    }
}
