using Xunit.Sdk;

namespace Benday.Common.Testing;

public class CheckAssertionFailureException : XunitException
{
    public CheckAssertionFailureException(string message)
        : base(message)
    {
    }
}