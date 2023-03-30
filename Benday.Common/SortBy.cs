namespace Benday.Common
{
    /// <summary>
    /// Defines a sorting rule for a property or column in a search result
    /// </summary>
    public class SortBy
    {
        /// <summary>
        /// Property name or column name to sort by
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;
        
        /// <summary>
        /// Direction to sort the property by -- ascending or descending
        /// </summary>
        public string Direction { get; set; } = SearchConstants.SortDirectionAscending;
    }
}
