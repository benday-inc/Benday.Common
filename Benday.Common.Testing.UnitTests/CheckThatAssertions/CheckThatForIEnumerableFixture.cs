using Benday.Common.Testing;

using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForIEnumerablesFixture : TestClassBase
{
    

    public CheckThatForIEnumerablesFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void CheckThat_IsEqualTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> expected = new List<ClassForTesting> { a, b, c };

        var check = input.CheckThatCollection();

        check.IsEqualTo(expected);
    }

    [Fact]
    public void CheckThat_IsEqualTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> expected = new List<ClassForTesting> { c, b, a };
        var check = input.CheckThatCollection();

        Assert.Throws<CheckAssertionFailureException>(() =>
        check.IsEqualTo(expected)
        );
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> notExpected = new List<ClassForTesting> { c, b, a };
        var check = input.CheckThatCollection();
        check.IsNotEqualTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEqualTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> notExpected = new List<ClassForTesting> { a, b, c };

        var check = input.CheckThatCollection();

        Assert.Throws<CheckAssertionFailureException>(() =>
            check.IsNotEqualTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> expected = new List<ClassForTesting> { c, a, b };
        var check = input.CheckThatCollection(); 

        check.IsEquivalentTo(expected);
    }

    [Fact]
    public void CheckThat_IsEquivalentTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> expected = new List<ClassForTesting> { a, b, d };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.IsEquivalentTo(expected)
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
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> notExpected = new List<ClassForTesting> { x, y, z };
        var check = input.CheckThatCollection();
        
        check.IsNotEquivalentTo(notExpected);
    }

    [Fact]
    public void CheckThat_IsNotEquivalentTo_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> notExpected = new List<ClassForTesting> { c, b, a };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.IsNotEquivalentTo(notExpected)
        );
    }

    [Fact]
    public void CheckThat_Contains_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        var check = input.CheckThatCollection(); 
        check.Contains(b);
    }

    [Fact]
    public void CheckThat_Contains_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.Contains(d)
        );
    }

    [Fact]
    public void CheckThat_DoesNotContain_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        var check = input.CheckThatCollection(); 

        check.DoesNotContain(d);
    }

    [Fact]
    public void CheckThat_DoesNotContain_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.DoesNotContain(b)
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting?> input = new List<ClassForTesting?> { a, b, c };
        var check = input.CheckThatCollection(); 
        check.AllItemsAreNotNull();
    }

    [Fact]
    public void CheckThat_AllItemsAreNotNull_False()
    {
        var a = new ClassForTesting();
        ClassForTesting? b = null;
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting?> input = new List<ClassForTesting?> { a, b, c };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.AllItemsAreNotNull()
        );
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        var check = input.CheckThatCollection(); 
        check.AllItemsAreUnique();
    }

    [Fact]
    public void CheckThat_AllItemsAreUnique_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, a };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
             check.AllItemsAreUnique()
        );
    }

    [Fact]
    public void CheckThat_IsSubsetOf_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b };
        IEnumerable<ClassForTesting> superset = new List<ClassForTesting> { a, b, c, d };
        var check = input.CheckThatCollection(); 
        check.IsSubsetOf(superset);
    }

    [Fact]
    public void CheckThat_IsSubsetOf_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        var e = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, e };
        IEnumerable<ClassForTesting> superset = new List<ClassForTesting> { a, b, c, d };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
             check.IsSubsetOf(superset)
        );
    }

    [Fact]
    public void CheckThat_IsSupersetOf_True()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c, d };
        IEnumerable<ClassForTesting> subset = new List<ClassForTesting> { b, c };
        var check = input.CheckThatCollection(); 
        check.IsSupersetOf(subset);
    }

    [Fact]
    public void CheckThat_IsSupersetOf_False()
    {
        var a = new ClassForTesting();
        var b = new ClassForTesting();
        var c = new ClassForTesting();
        var d = new ClassForTesting();
        IEnumerable<ClassForTesting> input = new List<ClassForTesting> { a, b, c };
        IEnumerable<ClassForTesting> subset = new List<ClassForTesting> { b, d };
        var check = input.CheckThatCollection();
        Assert.Throws<CheckAssertionFailureException>(() =>
            check.IsSupersetOf(subset)
        );
    }
}