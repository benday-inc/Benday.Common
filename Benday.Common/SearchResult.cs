using System.Collections.Generic;

namespace Benday.Common
{
    /// <summary>
    /// Data result from a search
    /// </summary>
    /// <typeparam name="T">Data type for the result items</typeparam>
    public class SearchResult<T>
    {
        /// <summary>
        /// Results of the search
        /// </summary>
        public virtual IList<T>? Results { get; set; }
        
        /// <summary>
        /// Search request that was used to execute this search
        /// </summary>
        public virtual Search? SearchRequest { get; set; }
    }
}
