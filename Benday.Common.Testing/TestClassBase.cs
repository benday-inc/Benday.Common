using Xunit.Abstractions;

namespace Benday.Common.Testing;

/// <summary>
/// Base class for XUnit-based test classes.
/// </summary>
public abstract class TestClassBase
{
    private readonly ITestOutputHelper _output;

    /// <summary>
    /// Constructor for the base class.
    /// </summary>
    /// <param name="output">An instance of the XUnit ITestOutputHelper. This typically is provided by the XUnit runner itself at execution.</param>
    public TestClassBase(ITestOutputHelper output)
    {
        _output = output;
    }

    /// <summary>
    /// Write a message to the test output.
    /// </summary>
    /// <param name="message"></param>
    public void WriteLine(string message)
    {
        _output.WriteLine(message);
    }
}
