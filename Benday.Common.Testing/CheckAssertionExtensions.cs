using Benday.Common;

namespace Benday.Common.Testing;

public static class CheckAssertionExtensions
{
    public static void FailWithOptionalMessage<T>(this ICheckAssertion<T> check, string? message = null)
    {
        if (check.FailureMessage.IsNullOrWhitespace() == false)
        {
            Fail(check, check.FailureMessage!);
        }
        else if (
            message.IsNullOrWhitespace() == false)
        {
            Fail(check, message!);
        }
        else
        {
            Fail(check, "Check assertion failed.");
        }
    }

    public static void Fail<T>(this ICheckAssertion<T> check, string message)
    {
        throw new CheckAssertionFailureException(message);
    }

    public static ICheckAssertion<T> IsNotNull<T>(this ICheckAssertion<T> check)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Input is null.");
        }

        return check;
    }

    public static ICheckAssertion<string> IsNotNullOrEmpty(this ICheckAssertion<string> check)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Input is null.");
        }

        if (string.IsNullOrEmpty(check.Input))
        {
            check.FailWithOptionalMessage("String is empty");
        }

        return check;
    }
    
    public static ICheckAssertion<string> IsNotNullOrWhitespace(this ICheckAssertion<string> check)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Input is null.");
        }

        if (string.IsNullOrWhiteSpace(check.Input))
        {
            check.FailWithOptionalMessage("String is empty or whitespace");
        }

        return check;
    }
}


