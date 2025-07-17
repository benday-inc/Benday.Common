using Xunit.Sdk;

namespace Benday.Common.Testing;

public class CheckAssertionFailureException : XunitException
{
    public CheckAssertionFailureException(string message)
        : base(message)
    {
    }

    public CheckAssertionFailureException(string userMessage, string expected, string actual)
        : base($"{userMessage}\nExpected: {expected}\nActual: {actual}")
    {
    }
}