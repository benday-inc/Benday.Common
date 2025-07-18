using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Benday.Common;

namespace Benday.Common.Testing;

public static class CheckAssertionExtensions
{
    [DoesNotReturn]
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

    [DoesNotReturn]
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

    public static ICheckAssertion<T> IsNull<T>(this ICheckAssertion<T> check,
        string? userFailureMessage = null)
    {
        if (check.Input != null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input should be null.");
        }

        return check;
    }

    public static ICheckAssertion<T> IsTheSameAs<T>(this ICheckAssertion<T> check,
        T? expected,
        string? userFailureMessage = null) where T : class
    {
        if (check.Input != expected)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should be the same reference.");
        }

        return check;
    }

    public static ICheckAssertion<T> IsNotTheSameAs<T>(this ICheckAssertion<T> check,
        T? expected,
        string? userFailureMessage = null) where T : class
    {
        if (check.Input == expected)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should be the same reference.");
        }

        return check;
    }
}