using System;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForStringsFixture : TestClassBase
{
    public CheckThatForStringsFixture(ITestOutputHelper output) : base(output)
    {

    }

    private string? ValueToTest { get; set; } = null;

    private ICheckAssertion<string>? _SystemUnderTest;
    public ICheckAssertion<string> SystemUnderTest
    {
        get
        {
            if (_SystemUnderTest == null)
            {
                _SystemUnderTest = ValueToTest.CheckThat();
            }

            if (_SystemUnderTest == null)
            {
                Assert.Fail("SystemUnderTest was still null after initialization.");
            }

            return _SystemUnderTest;
        }
    }

    [Fact]
    public void CheckThat_StringIsNotNullOrEmpty()
    {
        ValueToTest = "Hello, World!";
        SystemUnderTest.IsNotNullOrEmpty();
    }

    [Fact]
    public void CheckThat_Null_IsNotNullOrEmpty()
    {
        ValueToTest = null;
        Assert.Throws<CheckAssertionFailureException>(() => SystemUnderTest.IsNotNullOrEmpty());
    }

    [Fact]
    public void CheckThat_EmptyString_IsNotNullOrEmpty()
    {
        string? input = "";
        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThat().IsNotNullOrEmpty());
    }

    [Fact]
    public void CheckThat_WhitespaceString_IsNotNullOrWhitespace()
    {
        string? input = "   ";
        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThat().IsNotNullOrWhitespace());
    }

    [Fact]
    public void CheckThat_IsEqualTo_Equal()
    {
        // arrange
        var input = "asdf1234";
        var value2 = "asdf1234";

        // act
        input.CheckThat().IsEqualTo(value2);

        // assert
        // this worked if we didn't get an exception
    }

    [Fact]
    public void CheckThat_IsEqualTo_NotEqual()
    {
        // arrange
        var input = "asdf1234";
        var value2 = "asdf1234asdf";

        // act & assert
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsEqualTo(value2)
        );
    }

    [Fact]
    public void CheckThat_AreNotEqual_Equal()
    {
        // arrange
        var input = "asdf1234";
        var value2 = "asdf1234";

        // act & assert
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsNotEqualTo(value2)
        );
    }

    [Fact]
    public void CheckThat_AreNotEqual_NotEqual()
    {
 // arrange
        var input = "asdf1234";
        var value2 = "asdf1234asdfasdf";

        // act
        input.CheckThat().IsNotEqualTo(value2);

        // assert
        // this worked if we didn't get an exception
    }
}