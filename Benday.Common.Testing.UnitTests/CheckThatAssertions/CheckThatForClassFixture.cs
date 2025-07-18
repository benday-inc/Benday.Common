using System;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForClassFixture : TestClassBase
{
    public class ClassForTesting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public CheckThatForClassFixture(ITestOutputHelper output) : base(output)
    {

    }


    [Fact]
    public void CheckThat_Null_IsNotNull()
    {
        ClassForTesting? input = null;
        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThat().IsNotNull());
    }

    [Fact]
    public void CheckThat_NotNull_IsNotNull()
    {
        ClassForTesting? input = new();

        input.CheckThat().IsNotNull();
    }

    [Fact]
    public void CheckThat_Null_IsNull()
    {
        ClassForTesting? input = null;
        input.CheckThat().IsNull();
    }

    [Fact]
    public void CheckThat_NotNull_IsNull()
    {
        ClassForTesting? input = new();

        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThat().IsNull());
    }

    [Fact]
    public void CheckThat_NotTheSameReference_IsTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = new();

        Assert.Throws<CheckAssertionFailureException>(() => actual.CheckThat().IsTheSameAs(expected));
    }

    [Fact]
    public void CheckThat_SameReference_IsTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = actual!;

        actual.CheckThat().IsTheSameAs(expected);
    }

    [Fact]
    public void CheckThat_NotTheSameReference_IsNotTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = new();

        actual.CheckThat().IsNotTheSameAs(expected);
    }

    [Fact]
    public void CheckThat_SameReference_IsNotTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = actual!;

        Assert.Throws<CheckAssertionFailureException>(() => actual.CheckThat().IsNotTheSameAs(expected));
    }
}