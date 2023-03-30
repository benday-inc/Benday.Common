using System;

namespace Benday.Common
{
    /// <summary>
    /// Named argument to a search.
    /// </summary>
    public class SearchArgument
    {
        /// <summary>
        /// Constructor when the argument value is a string
        /// </summary>
        /// <param name="propertyName">Name of the property or search argument</param>
        /// <param name="method">Operator for the search</param>
        /// <param name="searchValue">Value for this argument</param>
        /// <param name="addAsOperator">Describes how this argument value should be logically combined with other arguments.  And or Or.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SearchArgument(
            string propertyName,
            SearchMethod method,
            string searchValue,
            SearchOperator addAsOperator = SearchOperator.And)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Method = method;
            SearchValue = searchValue ?? throw new ArgumentNullException(nameof(searchValue));
            Operator = addAsOperator;
        }

        /// <summary>
        /// Constructor when the argument value is an int
        /// </summary>
        /// <param name="propertyName">Name of the property or search argument</param>
        /// <param name="method">Operator for the search</param>
        /// <param name="searchValue">Value for this argument</param>
        /// <param name="addAsOperator">Describes how this argument value should be logically combined with other arguments.  And or Or.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SearchArgument(
            string propertyName,
            SearchMethod method,
            int searchValue,
            SearchOperator addAsOperator = SearchOperator.And)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Method = method;
            SearchValue = searchValue.ToString();
            SearchValueAsInt = searchValue;
            Operator = addAsOperator;
        }

        /// <summary>
        /// Name of the search argument or property
        /// </summary>
        public string PropertyName { get; set; }
        
        /// <summary>
        /// Operator for this search
        /// </summary>
        public SearchMethod Method { get; set; }
        
        /// <summary>
        /// Value for the search as string
        /// </summary>
        public string SearchValue { get; set; }
        
        /// <summary>
        /// Value for the search as integer
        /// </summary>
        public int SearchValueAsInt { get; set; }
        
        /// <summary>
        /// Describes how this argument should be logically combined
        /// with other arguments
        /// </summary>
        public SearchOperator Operator { get; set; }
    }
}
