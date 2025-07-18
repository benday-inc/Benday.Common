using System;

using Benday.Common.Testing;

using Xunit;

using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class FailureMessagesFixture : TestClassBase
{
    public FailureMessagesFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void CheckThat_FailureMessageIsUsed()
    {
        string? input = null;

        var check = input.CheckThat().WithMessage("Custom failure message");

        var exception = Assert.Throws<CheckAssertionFailureException>(() =>
            check.IsNotNullOrEmpty());

        Assert.Equal("Custom failure message", exception.Message);
    }

    // if check has a failure message, it should be used even if optional message is provided
    [Fact]
    public void CheckThat_FailureMessageIsUsedWithOptionalMessage()
    {
        string? input = null;
        var check = input.CheckThat().WithMessage("Custom failure message");

        var exception = Assert.Throws<CheckAssertionFailureException>(() =>
            check.FailWithOptionalMessage("This message should not be used"));

        Assert.Equal("Custom failure message", exception.Message);
    }

    // if check has no failure message, the provided optional message should be used
    [Fact]
    public void CheckThat_FailureMessageIsUsedWithOptionalMessageWhenNoFailureMessageSet()
    {
        string? input = null;
        var check = input.CheckThat();

        var exception = Assert.Throws<CheckAssertionFailureException>(() => check.FailWithOptionalMessage("This message should be used"));

        Assert.Equal("This message should be used", exception.Message);
    }

    [Fact]
    public void CheckThat_WithMessage_ValueIsUsed()
    {
        string? input = null;
        var check = input.CheckThat().WithMessage("Custom failure message for null input");

        var exception = Assert.Throws<CheckAssertionFailureException>(() => check.IsNotNullOrEmpty());
        Assert.Equal("Custom failure message for null input", exception.Message);
    }
    
    [Fact]
    public void CheckThat_DefaultFailureMessageIsUsed()
    {
        string? input = null;
        var check = input.CheckThat();     
        var exception = Assert.Throws<CheckAssertionFailureException>(() =>
            check.FailWithOptionalMessage(
                userFailureMessage: null,
                assertionDefaultMessage: "bing bong."
            ));
        Assert.Equal("bing bong.", exception.Message);
    }
}