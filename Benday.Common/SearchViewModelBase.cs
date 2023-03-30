using System.ComponentModel.DataAnnotations;

namespace Benday.Common
{
    /// <summary>
    /// Base class describing a view model for capturing the data for and 
    /// displaying the result of a search.
    /// </summary>
    /// <typeparam name="T">Data type for the result items</typeparam>
    public class SearchViewModelBase<T> : SortableViewModelBase<T>
    {
        public SearchViewModelBase()
        {
            IsSimpleSearch = true;
        }

        /// <summary>
        /// Is this a simple search. A simple search uses just a single string as a search argument
        /// </summary>
        [Display(Name = "Simple Search")]
        public bool IsSimpleSearch { get; set; }

        /// <summary>
        /// If this is a simple search, this is the search argument value
        /// </summary>
        [Display(Name = "Simple Search Value")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SimpleSearchValue { get; set; } = string.Empty;
    }
}
