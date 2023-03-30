using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Benday.Common
{
    /// <summary>
    /// Utility class that provides data paging for a result set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageableResults<T> : IPageableResults
    {
        private IList<T>? _results;
        private int _currentPage;

        public PageableResults()
        {
            ItemsPerPage = 10;
        }

        /// <summary>
        /// Sets the data result set that will be paged
        /// </summary>
        /// <param name="values">Result set to be paged</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Initialize(IList<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            Results = values;

            PageCount = CalculatePageCount();
            SetCurrentPage(1);
        }

        private void SetCurrentPage(int pageNumber)
        {
            if (pageNumber >= PageCount)
            {
                _currentPage = PageCount;
            }
            else if (pageNumber < 1)
            {
                _currentPage = 1;
            }
            else
            {
                _currentPage = pageNumber;
            }

            PopulatePageValues();
        }

        private void PopulatePageValues()
        {
            if (CurrentPage == 1)
            {
                PageValues = Results.Take(ItemsPerPage).ToList();
            }
            else
            {
                PageValues = Results
                    .Skip((CurrentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage).ToList();
            }
        }

        private int CalculatePageCount()
        {
            if (ItemsPerPage == 0)
            {
                return 0;
            }
            else if (ItemsPerPage < 0)
            {
                return 0;
            }
            else
            {
                var pageCount = TotalCount / ItemsPerPage;
                var remainder = TotalCount % ItemsPerPage;

                if (remainder == 0)
                {
                    return pageCount;
                }
                else
                {
                    return pageCount + 1;
                }
            }
        }

        /// <summary>
        /// The complete list of results wrapped for paging
        /// </summary>
        [JsonIgnore]
        public IList<T> Results
        {
            get
            {
                _results ??= new List<T>();

                return _results;
            }
            private set => _results = value;
        }

        /// <summary>
        /// Total number of items in the data source
        /// </summary>
        public int TotalCount => Results.Count;

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int ItemsPerPage { get; set; }
        
        /// <summary>
        /// Number of pages in the results
        /// </summary>
        public int PageCount { get; set; }
        
        /// <summary>
        /// Index of the current page being displayed
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            set => SetCurrentPage(value);
        }

        /// <summary>
        /// List of values in the current page
        /// </summary>
        public IList<T> PageValues { get; private set; } = new List<T>();
    }
}
