using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Benday.Common
{
    /// <summary>
    /// View model for displaying the results of a simple search.
    /// Provides basic properties for handling sorting of the results of the search
    /// and paging through the result set.
    /// </summary>
    /// <typeparam name="T">Data type for the result items</typeparam>
    public class SimpleSearchResults<T> : ISortableResult
    {
        public SimpleSearchResults()
        {
            CurrentSortProperty = string.Empty;
            CurrentSortDirection = SearchConstants.SortDirectionAscending;
        }

        /// <summary>
        /// Property name or column name for the result sort
        /// </summary>
        [Display(Name = "Current Sort Property")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrentSortProperty { get; set; }

        /// <summary>
        /// Direction for the result sort
        /// </summary>
        [Display(Name = "Current Sort Direction")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrentSortDirection { get; set; }

        /// <summary>
        /// Search value for simple search
        /// </summary>
        [JsonPropertyName("simpleSearchValue")]
        public string SearchValue { get; set; } = string.Empty;

        /// <summary>
        /// Index of the currently displayed page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of records in the result
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of items displayed per page
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Number of pages in the result
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// The result set values for the current page
        /// </summary>
        public IList<T> CurrentPageValues { get; set; } = new List<T>();
    }
}
