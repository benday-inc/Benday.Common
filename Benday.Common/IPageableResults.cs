namespace Benday.Common
{
    /// <summary>
    /// Represents information about a result set that can be
    /// arranged into pages. 
    /// </summary>
    public interface IPageableResults
    {
        /// <summary>
        /// Total number of records in the result
        /// </summary>
        int TotalCount { get; }
        
        /// <summary>
        /// Number of items per result page
        /// </summary>
        int ItemsPerPage { get; set; }
        
        /// <summary>
        /// Total number of pages in the result
        /// </summary>
        int PageCount { get; set; }
        
        /// <summary>
        /// Index of the current page in the result
        /// </summary>
        int CurrentPage { get; set; }
    }
}
