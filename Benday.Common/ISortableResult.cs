namespace Benday.Common
{
    /// <summary>
    /// Represents the sorting status of a result set 
    /// that allows sorting by single property value.
    /// </summary>
    public interface ISortableResult
    {
        /// <summary>
        /// Is the direction of the sort ascending or descending? 
        /// The acceptable values are defined in SearchConstants.
        /// </summary>
        string CurrentSortDirection { get; set; }
        
        /// <summary>
        /// The property being used as the sort value.
        /// </summary>
        string CurrentSortProperty { get; set; }
    }
}
