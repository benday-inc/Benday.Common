using System.Collections;
using System.Text;

namespace Benday.Common.Testing;

/// <summary>
/// Internal helper class for formatting assertion failure messages.
/// </summary>
internal static class AssertionMessageFormatter
{
    private const int MaxDisplayLength = 100;
    private const int MaxCollectionItems = 10;

    /// <summary>
    /// Formats a comparison message showing expected vs actual values.
    /// </summary>
    /// <typeparam name="T">The type of values being compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="userMessage">User-provided message.</param>
    /// <param name="operation">The operation being performed (e.g., "Equal", "NotEqual").</param>
    /// <returns>Formatted error message.</returns>
    public static string FormatComparisonMessage<T>(T expected, T actual, string userMessage, string operation)
    {
        string returnValue;

        if (!string.IsNullOrEmpty(userMessage))
        {
            returnValue = $"{userMessage} -- Assert.{operation} failed. Expected: {FormatValue(expected)}. Actual: {FormatValue(actual)}.";
        }
        else
        {
            returnValue = $"Assert.{operation} failed. Expected: {FormatValue(expected)}. Actual: {FormatValue(actual)}.";
        }
        
        return returnValue;
    }

    /// <summary>
    /// Formats a simple assertion message.
    /// </summary>
    /// <param name="userMessage">User-provided message.</param>
    /// <param name="operation">The operation being performed.</param>
    /// <param name="additionalInfo">Optional additional information.</param>
    /// <returns>Formatted error message.</returns>
    public static string FormatSimpleMessage(string userMessage, string operation, string? additionalInfo = null)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(userMessage))
        {
            sb.AppendLine(userMessage);
        }

        sb.AppendLine($"Assert.{operation} failed.");

        if (!string.IsNullOrEmpty(additionalInfo))
        {
            sb.AppendLine(additionalInfo);
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Formats a collection assertion message.
    /// </summary>
    /// <param name="collection">The collection being tested.</param>
    /// <param name="userMessage">User-provided message.</param>
    /// <param name="operation">The operation being performed.</param>
    /// <param name="additionalInfo">Optional additional information.</param>
    /// <returns>Formatted error message.</returns>
    public static string FormatCollectionMessage(IEnumerable? collection, string userMessage, string operation, string? additionalInfo = null)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(userMessage))
        {
            sb.AppendLine(userMessage);
        }

        sb.AppendLine($"CollectionAssert.{operation} failed.");

        if (collection != null)
        {
            sb.AppendLine($"Collection: {FormatValue(collection)}");
        }

        if (!string.IsNullOrEmpty(additionalInfo))
        {
            sb.AppendLine(additionalInfo);
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Formats a value for display in assertion messages.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>Formatted string representation of the value.</returns>
    public static string FormatValue(object? value)
    {
        if (value == null)
        {
            return "<null>";
        }

        if (value is string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str.Length == 0 ? "<empty string>" : "<null>";
            }

            var displayStr = str.Length > MaxDisplayLength
                ? str.Substring(0, MaxDisplayLength) + "..."
                : str;

            return $"\'{displayStr}\'";
        }

        if (value is IEnumerable enumerable && !(value is string))
        {
            return FormatCollection(enumerable);
        }

        var valueStr = value.ToString() ?? "<null>";

        if (valueStr.Length > MaxDisplayLength)
        {
            valueStr = valueStr.Substring(0, MaxDisplayLength) + "...";
        }

        return valueStr;
    }

    /// <summary>
    /// Formats a collection for display.
    /// </summary>
    /// <param name="collection">The collection to format.</param>
    /// <returns>Formatted string representation of the collection.</returns>
    private static string FormatCollection(IEnumerable collection)
    {
        var items = new List<string>();
        var count = 0;

        foreach (var item in collection)
        {
            if (count >= MaxCollectionItems)
            {
                items.Add("...");
                break;
            }

            items.Add(FormatValue(item));
            count++;
        }

        return $"[{string.Join(", ", items)}] (Count: {count}{(count >= MaxCollectionItems ? "+" : "")})";
    }

    /// <summary>
    /// Gets the type name for display purposes.
    /// </summary>
    /// <param name="type">The type to get the name for.</param>
    /// <returns>Friendly type name.</returns>
    public static string GetTypeName(Type type)
    {
        if (type.IsGenericType)
        {
            var genericTypeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));
            return $"{genericTypeName}<{genericArgs}>";
        }

        return type.Name;
    }
}