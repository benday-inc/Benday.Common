using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Benday.Common.Testing;

/// <summary>
/// Provides fluent assertion extension methods for strings.
/// </summary>
public static class StringAssertExtensions
{
    /// <summary>
    /// Verifies that the string is null or empty.
    /// </summary>
    /// <param name="actual">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string is not null or empty.</exception>
    public static string? ShouldBeNullOrEmpty([NotNull] this string? actual, string message)
    {
        StringAssert.IsNullOrEmpty(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string is not null or empty.
    /// </summary>
    /// <param name="actual">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string is null or empty.</exception>
    public static string ShouldNotBeNullOrEmpty([NotNull] this string? actual, string message)
    {
        StringAssert.IsNotNullOrEmpty(actual, message);
        return actual!;
    }

    /// <summary>
    /// Verifies that the string is null or whitespace.
    /// </summary>
    /// <param name="actual">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string is not null or whitespace.</exception>
    public static string? ShouldBeNullOrWhiteSpace([NotNull] this string? actual, string message)
    {
        StringAssert.IsNullOrWhiteSpace(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string is not null or whitespace.
    /// </summary>
    /// <param name="actual">The string to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string is null or whitespace.</exception>
    public static string ShouldNotBeNullOrWhiteSpace([NotNull] this string? actual, string message)
    {
        StringAssert.IsNotNullOrWhiteSpace(actual, message);
        return actual!;
    }

    /// <summary>
    /// Verifies that the string starts with the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not start with the expected substring.</exception>
    public static string ShouldStartWith(this string actual, string expected, string message)
    {
        StringAssert.StartsWith(actual, expected, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string starts with the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not start with the expected substring.</exception>
    public static string ShouldStartWith(this string actual, string expected, StringComparison comparisonType, string message)
    {
        StringAssert.StartsWith(actual, expected, comparisonType, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string ends with the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not end with the expected substring.</exception>
    public static string ShouldEndWith(this string actual, string expected, string message)
    {
        StringAssert.EndsWith(actual, expected, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string ends with the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not end with the expected substring.</exception>
    public static string ShouldEndWith(this string actual, string expected, StringComparison comparisonType, string message)
    {
        StringAssert.EndsWith(actual, expected, comparisonType, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string contains the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not contain the expected substring.</exception>
    public static string ShouldContain(this string actual, string expected, string message)
    {
        StringAssert.Contains(actual, expected, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string contains the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected substring.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not contain the expected substring.</exception>
    public static string ShouldContain(this string actual, string expected, StringComparison comparisonType, string message)
    {
        StringAssert.Contains(actual, expected, comparisonType, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string does not contain the expected substring.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The substring that should not be present.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string contains the substring.</exception>
    public static string ShouldNotContain(this string actual, string expected, string message)
    {
        StringAssert.DoesNotContain(actual, expected, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string does not contain the expected substring using the specified comparison.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The substring that should not be present.</param>
    /// <param name="comparisonType">The type of comparison to perform.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string contains the substring.</exception>
    public static string ShouldNotContain(this string actual, string expected, StringComparison comparisonType, string message)
    {
        StringAssert.DoesNotContain(actual, expected, comparisonType, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not match the pattern.</exception>
    public static string ShouldMatch(this string actual, string pattern, string message)
    {
        StringAssert.Matches(actual, pattern, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string matches the specified regular expression pattern with options.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not match the pattern.</exception>
    public static string ShouldMatch(this string actual, string pattern, RegexOptions options, string message)
    {
        StringAssert.Matches(actual, pattern, options, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string matches the pattern.</exception>
    public static string ShouldNotMatch(this string actual, string pattern, string message)
    {
        StringAssert.DoesNotMatch(actual, pattern, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string does not match the specified regular expression pattern with options.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="pattern">The regular expression pattern.</param>
    /// <param name="options">The regular expression options.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string matches the pattern.</exception>
    public static string ShouldNotMatch(this string actual, string pattern, RegexOptions options, string message)
    {
        StringAssert.DoesNotMatch(actual, pattern, options, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the string has the expected length.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expectedLength">The expected length.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the string does not have the expected length.</exception>
    public static string ShouldHaveLength(this string actual, int expectedLength, string message)
    {
        StringAssert.HasLength(actual, expectedLength, message);
        return actual;
    }

    /// <summary>
    /// Verifies that two strings are equal ignoring case.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="expected">The expected string.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the strings are not equal ignoring case.</exception>
    public static string ShouldEqualIgnoreCase(this string actual, string expected, string message)
    {
        StringAssert.AreEqualIgnoreCase(expected, actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that two strings are not equal ignoring case.
    /// </summary>
    /// <param name="actual">The actual string.</param>
    /// <param name="notExpected">The string that should not match the actual string.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual string for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the strings are equal ignoring case.</exception>
    public static string ShouldNotEqualIgnoreCase(this string actual, string notExpected, string message)
    {
        StringAssert.AreNotEqualIgnoreCase(notExpected, actual, message);
        return actual;
    }
}