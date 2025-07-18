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
    public void CheckThat_IsEqualCaseInsensitiveTo_Equal()
    {
        // arrange
        var input = "asdf1234";
        var value2 = "ASDF1234";

        // act
        input.CheckThat().IsEqualCaseInsensitiveTo(value2);

        // assert
        // this worked if we didn't get an exception
    }

    [Fact]
    public void CheckThat_IsEqualCaseInsensitiveTo_NotEqual()
    {
        // arrange
        var input = "asdf1234";
        var value2 = "ASDF1234asdf";

        // act & assert
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsEqualCaseInsensitiveTo(value2)
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

    [Fact]
    public void CheckThat_Contains_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().Contains("World");
    }

    [Fact]
    public void CheckThat_Contains_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().Contains("Universe")
        );
    }

    [Fact]
    public void CheckThat_DoesNotContain_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().DoesNotContain("Universe");
    }

    [Fact]
    public void CheckThat_DoesNotContain_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotContain("World")
        );
    }

    [Fact]
    public void CheckThat_StartsWith_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().StartsWith("Hello");
    }

    [Fact]
    public void CheckThat_StartsWith_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().StartsWith("World")
        );
    }

    [Fact]
    public void CheckThat_EndsWith_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().EndsWith("World!");
    }

    [Fact]
    public void CheckThat_EndsWith_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().EndsWith("Hello")
        );
    }

    [Fact]
    public void CheckThat_DoesNotStartWith_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().DoesNotStartWith("World");
    }

    [Fact]
    public void CheckThat_DoesNotStartWith_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotStartWith("Hello")
        );
    }

    [Fact]
    public void CheckThat_DoesNotEndWith_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().DoesNotEndWith("Hello");
    }

    [Fact]
    public void CheckThat_DoesNotEndWith_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotEndWith("World!")
        );
    }

    [Fact]
    public void CheckThat_ContainsCaseInsensitive_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().ContainsCaseInsensitive("world");
    }

    [Fact]
    public void CheckThat_ContainsCaseInsensitive_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().ContainsCaseInsensitive("universe")
        );
    }

    [Fact]
    public void CheckThat_DoesNotContainCaseInsensitive_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().DoesNotContainCaseInsensitive("universe");
    }

    [Fact]
    public void CheckThat_DoesNotContainCaseInsensitive_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotContainCaseInsensitive("world")
        );
    }

    [Fact]
    public void CheckThat_StartsWithCaseInsensitive_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().StartsWithCaseInsensitive("hello");
    }

    [Fact]
    public void CheckThat_StartsWithCaseInsensitive_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().StartsWithCaseInsensitive("world")
        );
    }

    [Fact]
    public void CheckThat_EndsWithCaseInsensitive_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().EndsWithCaseInsensitive("world!");
    }

    [Fact]
    public void CheckThat_EndsWithCaseInsensitive_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().EndsWithCaseInsensitive("hello")
        );
    }

    [Fact]
    public void CheckThat_DoesNotStartWithCaseInsensitive_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().DoesNotStartWithCaseInsensitive("world");
    }

    [Fact]
    public void CheckThat_DoesNotStartWithCaseInsensitive_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotStartWithCaseInsensitive("hello")
        );
    }

    [Fact]
    public void CheckThat_DoesNotEndWithCaseInsensitive_True()
    {
        string? input = "Hello, World!";
        input.CheckThat().DoesNotEndWithCaseInsensitive("hello");
    }

    [Fact]
    public void CheckThat_DoesNotEndWithCaseInsensitive_False()
    {
        string? input = "Hello, World!";
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotEndWithCaseInsensitive("world!")
        );
    }
}