namespace Benday.Common
{
    /// <summary>
    /// Extension methods for string.
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// This wraps the ToString() method and handles the case where the string is null.  Returns the input string if it is not null, otherwise returns the default value.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue">Value to return if null. The default value is empty string.</param>
        /// <returns></returns>
        public static string SafeToString(this string? input, string defaultValue = "")
        {
            if (input is null)
            {
                return defaultValue;
            }
            else
            {
                return input.ToString();
            }
        }
    }
}
