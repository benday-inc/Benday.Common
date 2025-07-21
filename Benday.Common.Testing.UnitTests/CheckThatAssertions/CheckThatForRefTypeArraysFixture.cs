using System;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForRefTypeArraysFixture : TestClassBase
{
    public CheckThatForRefTypeArraysFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void CheckThat_ThrowsException_WhenTypeIsArray()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var expected = new[] { input[0], input[1], input[2] };

        Assert.Throws<WrongCheckThatMethodException>(() =>
            input.CheckThat()
        );
    }

    [Fact]
    public void CheckThat_IsEqualTo_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };
        var expected = new[] { input[0], input[1], input[2] };

        var check = input.CheckThatArray();

        check.IsEqualTo(expected);
    }

    [Fact]
    public void CheckThat_IsEqualTo_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };
        var expected = new[] { input[1], input[2], input[0] };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsEqualTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() }
        .ToList();

        var notExpected = new[] { input[0], input[1], input[2] }.ToList();

        input.CheckThatCollection().IsNotEqualTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var notExpected = new[] { input[0], input[1], input[2] };

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsNotEqualTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };
        var expected = new[] { input[1], input[2], input[0] };
        input.CheckThatArray().IsEquivalentTo(expected);
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };
        var expected = new[] { input[1], input[2], new ClassForTesting() };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsEquivalentTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var notExpected = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        input.CheckThatArray().IsNotEquivalentTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };
        var notExpected = new[] { input[1], input[2], input[0] };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().IsNotEquivalentTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_Contains_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };
        var expectedItem = input[1];
        input.CheckThatArray().Contains(expectedItem);
    }

    [Fact]
    public void CheckThat_Contains_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var notExpectedItem = new ClassForTesting();

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().Contains(notExpectedItem)
        );
    }

    [Fact]
    public void CheckThat_DoesNotContain_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var notExpectedItem = new ClassForTesting();

        input.CheckThatArray().DoesNotContain(notExpectedItem);
    }

    [Fact]
    public void CheckThat_DoesNotContain_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var notExpectedItem = input[0];

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatArray().DoesNotContain(notExpectedItem)
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_True()
    {
        ClassForTesting?[] input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        input.CheckThatNullable().AllItemsAreNotNull();
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_False()
    {
        ClassForTesting?[] input = new[] {
            new ClassForTesting(),
            null,
            new ClassForTesting() };

        var check = input.CheckThatNullable();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.AllItemsAreNotNull()
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        input.CheckThat().AllItemsAreUnique();
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        input[2] = input[0]; // Make the last item a duplicate

        var check = input.CheckThat();

        Assert.Throws<CheckAssertionFailureException>(() =>
            check.AllItemsAreUnique()
        );
    }

    [Fact]
    public void CheckThat_IsSubsetOf_True()
    {
        var items = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var input = new[] { items[0], items[2] };
        var superset = new[] { 
            items[0],
            items[1],
            items[2],
            items[3] };

        input.CheckThat().IsSubsetOf(superset);
    }

    [Fact]
    public void CheckThat_IsSubsetOf_False()
    {
        var items = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var input = new[] { items[0], items[2], new ClassForTesting() };
        var superset = new[] {
            items[0],
            items[1],
            items[2],
            items[3] };

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsSubsetOf(superset)
        );
    }

    [Fact]
    public void CheckThat_IsSupersetOf_True()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var subset = new[] { input[2], input[3] };

        input.CheckThat().IsSupersetOf(subset);
    }

    [Fact]
    public void CheckThat_IsSupersetOf_False()
    {
        var input = new[] {
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting(),
            new ClassForTesting() };

        var subset = new[] { input[2], new ClassForTesting() };

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsSupersetOf(subset)
        );
    }
}