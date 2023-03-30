namespace Benday.Common
{
    /// <summary>
    /// Search method for a search argument
    /// </summary>
    public enum SearchMethod
    {
        /// <summary>
        /// Skip this argument
        /// </summary>
        Skip,
        /// <summary>
        /// Contains this value
        /// </summary>
        Contains,
        /// <summary>
        /// Does not contain this value
        /// </summary>
        DoesNotContain,
        /// <summary>
        /// Starts with this value
        /// </summary>
        StartsWith,
        /// <summary>
        /// Ends with this value
        /// </summary>
        EndsWith,
        /// <summary>
        /// Equals this value
        /// </summary>
        Equals,
        /// <summary>
        /// Is not equal to this value
        /// </summary>
        IsNotEqual
    }
}
