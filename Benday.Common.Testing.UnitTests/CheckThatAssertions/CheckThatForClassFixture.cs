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
    public void CheckThatCollection_Null_IsNotNull()
    {
        ClassForTesting? input = null;
        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThatCollection().IsNotNull());
    }

    [Fact]
    public void CheckThatCollection_NotNull_IsNotNull()
    {
        ClassForTesting? input = new();

        input.CheckThatCollection().IsNotNull();
    }

    [Fact]
    public void CheckThatCollection_Null_IsNull()
    {
        ClassForTesting? input = null;
        input.CheckThatCollection().IsNull();
    }

    [Fact]
    public void CheckThatCollection_NotNull_IsNull()
    {
        ClassForTesting? input = new();

        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThatCollection().IsNull());
    }

    [Fact]
    public void CheckThatCollection_NotTheSameReference_IsTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = new();

        Assert.Throws<CheckAssertionFailureException>(() => actual.CheckThatCollection().IsTheSameAs(expected));
    }

    [Fact]
    public void CheckThatCollection_SameReference_IsTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = actual!;

        actual.CheckThatCollection().IsTheSameAs(expected);
    }

    [Fact]
    public void CheckThatCollection_NotTheSameReference_IsNotTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = new();

        actual.CheckThatCollection().IsNotTheSameAs(expected);
    }

    [Fact]
    public void CheckThatCollection_SameReference_IsNotTheSameAs()
    {
        ClassForTesting? actual = new();
        ClassForTesting expected = actual!;

        Assert.Throws<CheckAssertionFailureException>(() => actual.CheckThatCollection().IsNotTheSameAs(expected));
    }
}