using System;
using System.Collections.Generic;
using Benday.Common.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForCollectionsFixture : TestClassBase
{
    public class ClassForTesting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
    
    public CheckThatForCollectionsFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void CheckThat_IsEqualTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var expected = new List<ClassForTesting> { a, b, c };

        var check = input.CheckThat();

        check.IsEqualTo(expected);
    }

    [Fact]
    public void CheckThat_IsEqualTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var expected = new List<ClassForTesting> { c, b, a };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsEqualTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var notExpected = new List<ClassForTesting> { c, b, a };
        input.CheckThat().IsNotEqualTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var notExpected = new List<ClassForTesting> { a, b, c };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsNotEqualTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var expected = new List<ClassForTesting> { c, a, b };
        input.CheckThat().IsEquivalentTo(expected);
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var expected = new List<ClassForTesting> { a, b, d };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsEquivalentTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var x = new ClassForTesting();
        var y = new ClassForTesting();
        var z = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var notExpected = new List<ClassForTesting> { x, y, z };
        input.CheckThat().IsNotEquivalentTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var notExpected = new List<ClassForTesting> { c, b, a };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsNotEquivalentTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_Contains_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        input.CheckThat().Contains(b);
    }

    [Fact]
    public void CheckThat_Contains_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().Contains(d)
        );
    }

    [Fact]
    public void CheckThat_DoesNotContain_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        input.CheckThat().DoesNotContain(d);
    }

    [Fact]
    public void CheckThat_DoesNotContain_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().DoesNotContain(b)
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting?> { a, b, c };
        input.CheckThat().AllItemsAreNotNull();
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_False()
    {
        var a = new ClassForTesting();
        ClassForTesting? b = null;
        var c = new ClassForTesting();
        var input = new List<ClassForTesting?> { a, b, c };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().AllItemsAreNotNull()
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        input.CheckThat().AllItemsAreUnique();
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, a };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().AllItemsAreUnique()
        );
    }

    [Fact]
    public void CheckThat_IsSubsetOf_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b };
        var superset = new List<ClassForTesting> { a, b, c, d };
        input.CheckThat().IsSubsetOf(superset);
    }

    [Fact]
    public void CheckThat_IsSubsetOf_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var e = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, e };
        var superset = new List<ClassForTesting> { a, b, c, d };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsSubsetOf(superset)
        );
    }

    [Fact]
    public void CheckThat_IsSupersetOf_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c, d };
        var subset = new List<ClassForTesting> { b, c };
        input.CheckThat().IsSupersetOf(subset);
    }

    [Fact]
    public void CheckThat_IsSupersetOf_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var input = new List<ClassForTesting> { a, b, c };
        var subset = new List<ClassForTesting> { b, d };
        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThat().IsSupersetOf(subset)
        );
    }
}