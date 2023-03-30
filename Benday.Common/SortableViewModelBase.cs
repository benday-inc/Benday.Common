using System.ComponentModel.DataAnnotations;

namespace Benday.Common
{
    /// <summary>
    /// Base class for a view model that provides data sorting capabilities for a result set
    /// </summary>
    /// <typeparam name="T">Data type for the result items</typeparam>
    public abstract class SortableViewModelBase<T> : ISortableResult
    {
        protected SortableViewModelBase()
        {
            CurrentSortProperty = string.Empty;
            CurrentSortDirection = SearchConstants.SortDirectionAscending;
        }

        /// <summary>
        /// Property name or column name to sort by
        /// </summary>
        [Display(Name = "Current Sort Property")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrentSortProperty { get; set; }

        /// <summary>
        /// Direction for the sort: Ascending or Descending
        /// </summary>
        [Display(Name = "Current Sort Direction")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrentSortDirection { get; set; }

        /// <summary>
        /// Results of the search wrapped in a wrapper that provides 
        /// paging services
        /// </summary>
        public PageableResults<T>? Results
        {
            get;
            set;
        }
    }
}
