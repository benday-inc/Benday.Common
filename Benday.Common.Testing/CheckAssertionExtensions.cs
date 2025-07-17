using System.Runtime.CompilerServices;

using Benday.Common;

namespace Benday.Common.Testing;

public static class CheckAssertionExtensions
{
    public static void FailWithOptionalMessage<T>(this ICheckAssertion<T> check,
        string? userFailureMessage = null,
        string assertionDefaultMessage = "Check assertion failed.")
    {
        if (check.FailureMessage.IsNullOrWhitespace() == false)
        {
            // failure message is set, use it
            Fail(check, check.FailureMessage!);
        }
        else if (
            userFailureMessage.IsNullOrWhitespace() == false)
        {
            // failure message is not set and user provided a failure message, use it
            Fail(check, userFailureMessage!);
        }
        else
        {
            // no failure message set, use the default assertion message
            Fail(check, assertionDefaultMessage);
        }
    }

    public static void Fail<T>(this ICheckAssertion<T> check, string message)
    {
        throw new CheckAssertionFailureException(message);
    }

    public static ICheckAssertion<T> IsNotNull<T>(this ICheckAssertion<T> check,
        string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
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


