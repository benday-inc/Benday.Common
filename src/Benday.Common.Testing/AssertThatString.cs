using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Benday.Common.Testing;

/// <summary>
/// Provides static methods for making assertions on strings with descriptive failure messages.
/// </summary>
public static class AssertThatString
{
    /// <summary>
    /// Verifies that the string is null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string is not null or empty.</exception>
    public static void IsNullOrEmpty(string? value, string message)
    {
        if (!string.IsNullOrEmpty(value))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage("<null or empty>", value, message, "IsNullOrEmpty"));
        }
    }

    /// <summary>
    /// Verifies that the string is not null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string is null or empty.</exception>
    public static void IsNotNullOrEmpty([NotNull] string? value, string message)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage("<not null or empty>", value, message, "IsNotNullOrEmpty"));
        }
    }

    /// <summary>
    /// Verifies that the string is null or whitespace.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string is not null or whitespace.</exception>
    public static void IsNullOrWhiteSpace(string? value, string message)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage("<null or whitespace>", value, message, "IsNullOrWhiteSpace"));
        }
    }

    /// <summary>
    /// Verifies that the string is not null or whitespace.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string is null or whitespace.</exception>
    public static void IsNotNullOrWhiteSpace([NotNull] string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage("<not null or whitespace>", value, message, "IsNotNullOrWhiteSpace"));
        }
    }

    /// <summary>
    /// Verifies that the string starts with the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not start with the expected substring.</exception>
    public static void StartsWith(string actual, string expected, string message)
    {
        StartsWith(actual, expected, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Verifies that the string starts with the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not start with the expected substring.</exception>
    public static void StartsWith(string actual, string expected, StringComparison comparisonType, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (!actual.StartsWith(expected, comparisonType))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "StartsWith",
                $"Expected string to start with: {AssertionMessageFormatter.FormatValue(expected)}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that the string ends with the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not end with the expected substring.</exception>
    public static void EndsWith(string actual, string expected, string message)
    {
        EndsWith(actual, expected, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Verifies that the string ends with the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not end with the expected substring.</exception>
    public static void EndsWith(string actual, string expected, StringComparison comparisonType, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (!actual.EndsWith(expected, comparisonType))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "EndsWith",
                $"Expected string to end with: {AssertionMessageFormatter.FormatValue(expected)}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that the string contains the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not contain the expected substring.</exception>
    public static void Contains(string actual, string expected, string message)
    {
        Contains(actual, expected, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Verifies that the string contains the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not contain the expected substring.</exception>
    public static void Contains(string actual, string expected, StringComparison comparisonType, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (!actual.Contains(expected, comparisonType))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "Contains",
                $"Expected string to contain: {AssertionMessageFormatter.FormatValue(expected)}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that the string does not contain the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The substring that should not be present.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string contains the substring.</exception>
    public static void DoesNotContain(string actual, string expected, string message)
    {
        DoesNotContain(actual, expected, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Verifies that the string does not contain the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The substring that should not be present.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string contains the substring.</exception>
    public static void DoesNotContain(string actual, string expected, StringComparison comparisonType, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual.Contains(expected, comparisonType))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "DoesNotContain",
                $"Expected string not to contain: {AssertionMessageFormatter.FormatValue(expected)}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not match the pattern.</exception>
    public static void Matches(string actual, string pattern, string message)
    {
        Matches(actual, pattern, RegexOptions.None, message);
    }

    /// <summary>
    /// Verifies that the string matches the specified regular expression pattern with options.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not match the pattern.</exception>
    public static void Matches(string actual, string pattern, RegexOptions options, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        if (!Regex.IsMatch(actual, pattern, options))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "Matches",
                $"Expected string to match pattern: {AssertionMessageFormatter.FormatValue(pattern)}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string matches the pattern.</exception>
    public static void DoesNotMatch(string actual, string pattern, string message)
    {
        DoesNotMatch(actual, pattern, RegexOptions.None, message);
    }

    /// <summary>
    /// Verifies that the string does not match the specified regular expression pattern with options.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string matches the pattern.</exception>
    public static void DoesNotMatch(string actual, string pattern, RegexOptions options, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        if (Regex.IsMatch(actual, pattern, options))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "DoesNotMatch",
                $"Expected string not to match pattern: {AssertionMessageFormatter.FormatValue(pattern)}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that the string has the expected length.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expectedLength">The expected length.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the string does not have the expected length.</exception>
    public static void HasLength(string actual, int expectedLength, string message)
    {
        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        if (actual.Length != expectedLength)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "HasLength",
                $"Expected length: {expectedLength}, Actual length: {actual.Length}\nActual string: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that two strings are equal ignoring case.
    /// </summary>
    /// <param name="expected">The expected string.</param>
    /// <param name="actual">The actual string.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the strings are not equal ignoring case.</exception>
    public static void AreEqualIgnoreCase(string expected, string actual, string message)
    {
        if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEqualIgnoreCase"));
        }
    }

    /// <summary>
    /// Verifies that two strings are not equal ignoring case.
    /// </summary>
    /// <param name="expected">The string that should not match the actual string.</param>
    /// <param name="actual">The actual string.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the strings are equal ignoring case.</exception>
    public static void AreNotEqualIgnoreCase(string expected, string actual, string message)
    {
        if (string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreNotEqualIgnoreCase"));
        }
    }
}