using System.ComponentModel;

namespace UntitledArticles.API.Domain.Enums
{
    /// <summary>
    /// Defines sort order
    /// </summary>
    public enum SortOrder
    {
        [Description("The records should be sort from the lowest to the highest")]
        ASC = 0,

        [Description("The records should be sort from the highest to the lowest")]
        DESC = 1,
    }
}
