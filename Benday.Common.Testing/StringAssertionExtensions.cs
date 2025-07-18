namespace Benday.Common.Testing;

public static class StringAssertionExtensions
{
    public static ICheckAssertion<string> IsEqualTo(this ICheckAssertion<string> check,
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

    public static ICheckAssertion<string> IsEqualCaseInsensitiveTo(this ICheckAssertion<string> check,
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

    public static ICheckAssertion<string> IsNotEqualTo(this ICheckAssertion<string> check,
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
    
    public static ICheckAssertion<string> IsNotNullOrEmpty(
        this ICheckAssertion<string> check,
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

    public static ICheckAssertion<string> IsNotNullOrWhitespace(this ICheckAssertion<string> check,
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
}


