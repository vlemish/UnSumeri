namespace AnSumeri.API.Domain.Search.Enums;

/// <summary>
/// A mode to use during the search
/// </summary>
public enum SearchMode
{
    /// <summary>
    /// The search will be performed by given search pattern across all properties
    /// </summary>
    Across = 0,

    /// <summary>
    /// The search will be performed by using logical AND (e.g. a= "text" and b = "other")
    /// </summary>
    AllTrue = 1,

    /// <summary>
    /// The search will be performed by using logical OR (e.g a = "text" or b = "other")
    /// </summary>
    SomeTrue = 2,

    /// <summary>
    /// The search will be performed by specified property
    /// </summary>
    SingleProperty = 3
}
