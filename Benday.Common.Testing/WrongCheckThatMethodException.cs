using Xunit.Sdk;

namespace Benday.Common.Testing;

public class WrongCheckThatMethodException : XunitException
{
    public WrongCheckThatMethodException(string message)
        : base(message)
    {
    }
}