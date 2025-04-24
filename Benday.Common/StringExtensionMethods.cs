using System;

namespace Benday.Common;

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

    /// <summary>
    /// This method compares two strings for equality, ignoring case.  If both strings are null, they are considered equal.
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool EqualsCaseInsensitive(this string? value1, string? value2)
    {
        if (value1 == null && value2 == null)
        {
            return true;
        }
        else if (value1 == null || value2 == null)
        {
            return false;
        }
        else if (string.Equals(value1, value2,
            StringComparison.CurrentCultureIgnoreCase) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int SafeToInt32(this string? value, int defaultValue = default)
    {
        var valueAsString = value.SafeToString();

        if (valueAsString == string.Empty)
        {
            return defaultValue;
        }
        else
        {
            if (int.TryParse(valueAsString, out var returnValue) == true)
            {
                return returnValue;
            }
            else
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// This method checks if the string contains a value.  If the string is null or empty, it returns false.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="containsValue"></param>
    /// <returns></returns>
    public static bool SafeContains(this string? value, string containsValue)
    {
        var valueAsString = value.SafeToString();

        if (valueAsString == string.Empty)
        {
            return false;
        }
        else
        {
            return valueAsString.Contains(containsValue);
        }
    }

    /// <summary>
    /// This method checks if the string contains a value ignoring case.  If the string is null or empty, it returns false.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="containsValue"></param>
    /// <returns></returns>
    public static bool SafeContainsCaseInsensitive(this string? value, string containsValue)
    {
        var valueAsString = value.SafeToString();

        if (valueAsString == string.Empty)
        {
            return false;
        }
        else
        {
            return valueAsString.Contains(containsValue, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    /// <summary>
    /// This method checks if the string is null or empty.  If the string is null or empty, it returns true.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrWhitespace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}
