using Benday.Common;

namespace Benday.Common.Testing;

public static class CheckAssertionExtensions
{
    public static void Fail<T>(this ICheckAssertion<T> check)
    {
        if (check.FailureMessage.IsNullOrWhitespace() == true)
        {
            Fail(check, "Check assertion failed.");
        }
        else
        {
            Fail(check, check.FailureMessage);
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
            throw new ArgumentNullException(nameof(check.Input));
        }

        return check;
    }

    public static ICheckAssertion<string> IsNotNullOrEmpty(this ICheckAssertion<string> check)
    {
        if (check.Input == null)
        {
            throw new ArgumentNullException(nameof(check.Input));
        }

        if (string.IsNullOrEmpty(check.Input))
        {
            throw new ArgumentException("String is null or empty", nameof(check.Input));
        }

        return check;
    }
}


