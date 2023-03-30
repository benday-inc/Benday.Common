using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Common
{
    /// <summary>
    /// Describes the data required to run a search
    /// </summary>
    public class Search
    {
        public Search()
        {
            Arguments = new List<SearchArgument>();
            Sorts = new List<SortBy>();
            MaxNumberOfResults = -1;
            Name = string.Empty;
        }

        /// <summary>
        /// Name of the search to run.  (Empty string or null string is the default.)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Search arguments and values for those arguments
        /// </summary>
        public List<SearchArgument> Arguments { get; }


        /// <summary>
        /// Add a search argument
        /// </summary>
        /// <param name="propertyName">Name of the search argument or property name</param>
        /// <param name="method">Type of search operator for this argument</param>
        /// <param name="value">Value for this search argument</param>
        /// <param name="combineWithOtherArgumentsAs">Describes how this argument is combined with other arguments: Or and And</param>
        public void AddArgument(
            string propertyName,
            SearchMethod method,
            string value,
            SearchOperator combineWithOtherArgumentsAs = SearchOperator.And)
        {
            Arguments.Add(
                new SearchArgument(propertyName, method, value, combineWithOtherArgumentsAs));
        }

        /// <summary>
        /// Add a search argument
        /// </summary>
        /// <param name="propertyName">Name of the search argument or property name</param>
        /// <param name="method">Type of search operator for this argument</param>
        /// <param name="value">Value for this search argument</param>
        /// <param name="combineWithOtherArgumentsAs">Describes how this argument is combined with other arguments: Or and And</param>
        public void AddArgument(
            string propertyName,
            SearchMethod method,
            int value,
            SearchOperator combineWithOtherArgumentsAs = SearchOperator.And)
        {
            Arguments.Add(
                new SearchArgument(propertyName, method, value, combineWithOtherArgumentsAs));
        }

        /// <summary>
        /// Maximum number of items in the result
        /// </summary>
        public int MaxNumberOfResults { get; set; }

        /// <summary>
        /// Description of how the result should be sorted
        /// </summary>
        public List<SortBy> Sorts { get; set; }

        /// <summary>
        /// Add a sort directive for the results of the search
        /// </summary>
        /// <param name="sortByPropertyName">Name of the result property to order by</param>
        /// <param name="direction">Ascending or descending</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AddSort(string sortByPropertyName,
            string direction = SearchConstants.SortDirectionAscending)
        {
            if (sortByPropertyName is null)
            {
                throw new System.ArgumentNullException(nameof(sortByPropertyName));
            }

            if (direction is null)
            {
                throw new System.ArgumentNullException(nameof(direction));
            }

            string directionCleaned;

            if (string.Compare(direction,
                SearchConstants.SortDirectionAscending, true) == 0)
            {
                directionCleaned = SearchConstants.SortDirectionAscending;
            }
            else if (string.Compare(direction,
                SearchConstants.SortDirectionDescending, true) == 0)
            {
                directionCleaned = SearchConstants.SortDirectionDescending;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(direction),
                    string.Format("Value should be '{0}' or '{1}'.",
                    SearchConstants.SortDirectionAscending,
                    SearchConstants.SortDirectionDescending));
            }

            AddSort(new SortBy()
            {
                PropertyName = sortByPropertyName,
                Direction = directionCleaned
            });
        }

        /// <summary>
        /// Add a sort directive for the results of the search
        /// </summary>
        /// <param name="sortBy"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void AddSort(SortBy sortBy)
        {
            if (sortBy is null)
            {
                throw new ArgumentNullException(nameof(sortBy));
            }

            var match = (from temp in Sorts
                         where
                         string.Compare(temp.PropertyName, sortBy.PropertyName, true) == 0
                         select temp).FirstOrDefault();

            if (match == null)
            {
                Sorts.Add(sortBy);
            }
        }
    }
}
