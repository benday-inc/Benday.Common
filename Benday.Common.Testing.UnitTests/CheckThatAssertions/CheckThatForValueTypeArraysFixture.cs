using Benday.Common.Testing;

using System;

using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForValueTypeArraysFixture : TestClassBase
{
    public CheckThatForValueTypeArraysFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void CheckThat_IsEqualTo_True()
    {
        var input = new[] { 1, 2, 3 };
        var expected = new[] { 1, 2, 3 };
        var check = input.CheckThatArray();

        check.IsEqualTo(expected);
    }

    [Fact]
    public void CheckThat_ThrowsException_WhenTypeIsArray()
    {
        var input = new[] { 1, 2, 3 };
        var expected = new[] { 1, 2, 3 };

        Assert.Throws<WrongCheckThatMethodException>(() =>
            input.CheckThat()
        );
    }

    [Fact]
    public void CheckThat_IsEqualTo_False()
    {
        var input = new[] { 1, 2, 3 };
        var expected = new[] { 3, 2, 1 };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsEqualTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_True()
    {
        var input = new[] { 1, 2, 3 };
        var notExpected = new[] { 3, 2, 1 };
        input.CheckThatArray().IsNotEqualTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_False()
    {
        var input = new[] { 1, 2, 3 };
        var notExpected = new[] { 1, 2, 3 };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsNotEqualTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_True()
    {
        var input = new[] { "a", "b", "c" };
        var expected = new[] { "c", "a", "b" };
        input.CheckThatArray().IsEquivalentTo(expected);
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_False()
    {
        var input = new[] { "a", "b", "c" };
        var expected = new[] { "a", "b", "d" };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsEquivalentTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_True()
    {
        var input = new[] { "a", "b", "c" };
        var notExpected = new[] { "x", "y", "z" };
        input.CheckThatArray().IsNotEquivalentTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_False()
    {
        var input = new[] { "a", "b", "c" };
        var notExpected = new[] { "c", "b", "a" };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsNotEquivalentTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_Contains_True()
    {
        var input = new[] { 1, 2, 3 };
        input.CheckThatArray().Contains(2);
    }

    [Fact]
    public void CheckThat_Contains_False()
    {
        var input = new[] { 1, 2, 3 };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().Contains(4)
        );
    }

    [Fact]
    public void CheckThat_DoesNotContain_True()
    {
        var input = new[] { 1, 2, 3 };
        input.CheckThatArray().DoesNotContain(4);
    }

    [Fact]
    public void CheckThat_DoesNotContain_False()
    {
        var input = new[] { 1, 2, 3 };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().DoesNotContain(2)
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_True()
    {
        var input = new[] { "a", "b", "c" };
        input.CheckThatArray().AllItemsAreNotNull();
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_False()
    {
        var input = new string?[] { "a", null, "c" };
        var check = input.CheckThatNullable();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.AllItemsAreNotNull()
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_True()
    {
        var input = new[] { "apple", "banana", "cherry" };
        input.CheckThatArray().AllItemsAreUnique();
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_False()
    {
        var input = new[] { "apple", "banana", "apple" };

        var check = input.CheckThatArray();

        Assert.Throws<CheckAssertionFailureException>(() =>
            check.AllItemsAreUnique()
        );
    }

    [Fact]
    public void CheckThat_IsSubsetOf_True()
    {
        var input = new[] { 1, 2 };
        var superset = new[] { 1, 2, 3, 4 };
        input.CheckThatArray().IsSubsetOf(superset);
    }

    [Fact]
    public void CheckThat_IsSubsetOf_False()
    {
        var input = new[] { 1, 2, 5 };
        var superset = new[] { 1, 2, 3, 4 };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsSubsetOf(superset)
        );
    }

    [Fact]
    public void CheckThat_IsSupersetOf_True()
    {
        var input = new[] { 1, 2, 3, 4 };
        var subset = new[] { 2, 3 };
        input.CheckThatArray().IsSupersetOf(subset);
    }

    [Fact]
    public void CheckThat_IsSupersetOf_False()
    {
        var input = new[] { 1, 2, 3 };
        var subset = new[] { 2, 4 };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsSupersetOf(subset)
        );
    }
}
