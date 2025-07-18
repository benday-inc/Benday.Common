using System.Diagnostics.CodeAnalysis;

namespace Benday.Common.Testing;

public static class NullableStringAssertionExtensions
{
    public static ICheckAssertionForNullableType<string?> IsEqualTo(this ICheckAssertionForNullableType<string?> check,
            string? expected,
            string? userFailureMessage = null)
    {
        if (string.Equals(check.Input, expected) == false)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should be equal. Expected '{expected}' but actual value was '{check.Input}'");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> IsEqualCaseInsensitiveTo(this ICheckAssertionForNullableType<string?> check,
        string? expected,
        string? userFailureMessage = null)
    {
        if (string.Equals(check.Input, expected, StringComparison.CurrentCultureIgnoreCase) == false)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should be equal. Expected '{expected}' but actual value was '{check.Input}'");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> IsNotEqualTo(this ICheckAssertionForNullableType<string?> check,
        string? expected,
        string? userFailureMessage = null)
    {
        if (string.Equals(check.Input, expected) == true)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should not be equal. Expected '{expected}' and actual value was '{check.Input}'");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> IsNotNullOrEmpty(
        this ICheckAssertionForNullableType<string?> check,
        string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (string.IsNullOrEmpty(check.Input))
        {
            check.FailWithOptionalMessage(userFailureMessage, "String is empty");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> IsNotNullOrWhitespace(
        this ICheckAssertionForNullableType<string?> check,
        string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (string.IsNullOrWhiteSpace(check.Input))
        {
            check.FailWithOptionalMessage(userFailureMessage, "String is empty or whitespace");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> Contains(this ICheckAssertionForNullableType<string?> check, string expected, string? userFailureMessage = null)
    {
        if (check.Input == null || check.Input.Contains(expected) == false)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to contain '{expected}'.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> ContainsCaseInsensitive(this ICheckAssertionForNullableType<string?> check, string expected, string? userFailureMessage = null)
    {
        if (check.Input == null || check.Input.IndexOf(expected, StringComparison.CurrentCultureIgnoreCase) < 0)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to contain '{expected}' (case-insensitive).");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> DoesNotContain(this ICheckAssertionForNullableType<string?> check, string unexpected, string? userFailureMessage = null)
    {
        if (check.Input != null && check.Input.Contains(unexpected))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to NOT contain '{unexpected}'.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> DoesNotContainCaseInsensitive(this ICheckAssertionForNullableType<string?> check, string unexpected, string? userFailureMessage = null)
    {
        if (check.Input != null && check.Input.IndexOf(unexpected, StringComparison.CurrentCultureIgnoreCase) >= 0)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to NOT contain '{unexpected}' (case-insensitive).");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> StartsWith(this ICheckAssertionForNullableType<string?> check, string expectedStart, string? userFailureMessage = null)
    {
        if (check.Input == null || check.Input.StartsWith(expectedStart) == false)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to start with '{expectedStart}'.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> StartsWithCaseInsensitive(this ICheckAssertionForNullableType<string?> check, string expectedStart, string? userFailureMessage = null)
    {
        if (check.Input == null || check.Input.StartsWith(expectedStart, StringComparison.CurrentCultureIgnoreCase) == false)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to start with '{expectedStart}' (case-insensitive).");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> DoesNotStartWith(this ICheckAssertionForNullableType<string?> check, string unexpectedStart, string? userFailureMessage = null)
    {
        if (check.Input != null && check.Input.StartsWith(unexpectedStart))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to NOT start with '{unexpectedStart}'.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> DoesNotStartWithCaseInsensitive(this ICheckAssertionForNullableType<string?> check, string unexpectedStart, string? userFailureMessage = null)
    {
        if (check.Input != null && check.Input.StartsWith(unexpectedStart, StringComparison.CurrentCultureIgnoreCase))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to NOT start with '{unexpectedStart}' (case-insensitive).");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> EndsWith(this ICheckAssertionForNullableType<string?> check, string expectedEnd, string? userFailureMessage = null)
    {
        if (check.Input == null || check.Input.EndsWith(expectedEnd) == false)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to end with '{expectedEnd}'.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> EndsWithCaseInsensitive(this ICheckAssertionForNullableType<string?> check, string expectedEnd, string? userFailureMessage = null)
    {
        if (check.Input == null || check.Input.EndsWith(expectedEnd, StringComparison.CurrentCultureIgnoreCase) == false)
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to end with '{expectedEnd}' (case-insensitive).");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> DoesNotEndWith(this ICheckAssertionForNullableType<string?> check, string unexpectedEnd, string? userFailureMessage = null)
    {
        if (check.Input != null && check.Input.EndsWith(unexpectedEnd))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to NOT end with '{unexpectedEnd}'.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<string?> DoesNotEndWithCaseInsensitive(this ICheckAssertionForNullableType<string?> check, string unexpectedEnd, string? userFailureMessage = null)
    {
        if (check.Input != null && check.Input.EndsWith(unexpectedEnd, StringComparison.CurrentCultureIgnoreCase))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected '{check.Input}' to NOT end with '{unexpectedEnd}' (case-insensitive).");
        }

        return check;
    }


}