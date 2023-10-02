namespace Benday.Common
{
    public static class StringExtensionMethods
    {
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
