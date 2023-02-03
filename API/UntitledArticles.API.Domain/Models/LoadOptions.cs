using UntitledArticles.API.Domain.Enums;

namespace UntitledArticles.API.Domain.Models
{
    /// <summary>
    /// Load Options
    /// </summary>
    public class LoadOptions
    {
        /// <summary>
        /// Sort Order
        /// </summary>
        public SortOrder SortOrder { get; } = SortOrder.ASC;

        /// <summary>
        /// Number of rows to skip
        /// </summary>
        public int SkipRows { get; }

        /// <summary>
        /// Number of rows to take up to
        /// </summary>
        public int? MaxRows { get; }

        #region Ctors

        public LoadOptions(int skipRows, int? maxRows, SortOrder sortOrder)
        {
            SkipRows = skipRows;
            MaxRows = maxRows;
            SortOrder = sortOrder;
        }

        public LoadOptions(int skipRows, int? maxRows)
        {
            SkipRows = skipRows;
            MaxRows = maxRows;
        }

        public LoadOptions(int skipRows, SortOrder sortOrder)
        {
            SkipRows = skipRows;
            SortOrder = sortOrder;
        }

        public LoadOptions(int skipRows)
        {
            SkipRows = skipRows;
        }

        #endregion
    }
}
