using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Extensions;

public class CollectionAssertExtensionsFixture : TestClassBase
{
    public CollectionAssertExtensionsFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region ShouldBeEmpty Tests

    [Fact]
    public void ShouldBeEmpty_WithEmptyCollection_ReturnsCollection()
    {
        // Arrange
        var actual = new List<int>();

        // Act
        var result = actual.ShouldBeEmpty("Should be empty");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldBeEmpty_WithNonEmptyCollection_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 3 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeEmpty("Should be empty"));

        Assert.Contains("Should be empty", ex.Message);
    }

    #endregion

    #region ShouldNotBeEmpty Tests

    [Fact]
    public void ShouldNotBeEmpty_WithNonEmptyCollection_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 1, 2, 3 };

        // Act
        var result = actual.ShouldNotBeEmpty("Should not be empty");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldNotBeEmpty_WithEmptyCollection_Throws()
    {
        // Arrange
        var actual = new List<string>();

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeEmpty("Should not be empty"));

        Assert.Contains("Should not be empty", ex.Message);
    }

    #endregion

    #region ShouldHaveCount Tests

    [Fact]
    public void ShouldHaveCount_WithCorrectCount_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { "a", "b", "c" };

        // Act
        var result = actual.ShouldHaveCount(3, "Should have 3 items");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldHaveCount_WithWrongCount_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldHaveCount(3, "Wrong count"));

        Assert.Contains("Wrong count", ex.Message);
    }

    #endregion

    #region ShouldContain Tests

    [Fact]
    public void ShouldContain_WithPresentItem_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { "apple", "banana", "cherry" };

        // Act
        var result = actual.ShouldContain("banana", "Should contain banana");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldContain_WithAbsentItem_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 3 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldContain(4, "Should contain 4"));

        Assert.Contains("Should contain 4", ex.Message);
    }

    #endregion

    #region ShouldNotContain Tests

    [Fact]
    public void ShouldNotContain_WithAbsentItem_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 1, 2, 3 };

        // Act
        var result = actual.ShouldNotContain(4, "Should not contain 4");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldNotContain_WithPresentItem_Throws()
    {
        // Arrange
        var actual = new[] { "a", "b", "c" };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotContain("b", "Should not contain b"));

        Assert.Contains("Should not contain b", ex.Message);
    }

    #endregion

    #region ShouldAllMatch Tests

    [Fact]
    public void ShouldAllMatch_WhenAllMatch_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 2, 4, 6, 8 };

        // Act
        var result = actual.ShouldAllMatch(x => x % 2 == 0, "All should be even");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldAllMatch_WhenSomeDoNotMatch_Throws()
    {
        // Arrange
        var actual = new[] { 2, 3, 4, 5 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldAllMatch(x => x % 2 == 0, "All should be even"));

        Assert.Contains("All should be even", ex.Message);
    }

    #endregion

    #region ShouldAnyMatch Tests

    [Fact]
    public void ShouldAnyMatch_WhenSomeMatch_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 1, 2, 3, 4 };

        // Act
        var result = actual.ShouldAnyMatch(x => x > 3, "Some should be > 3");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldAnyMatch_WhenNoneMatch_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 3 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldAnyMatch(x => x > 10, "Some should be > 10"));

        Assert.Contains("Some should be > 10", ex.Message);
    }

    #endregion

    #region ShouldEqual Tests

    // Note: ShouldEqual for collections has a conflict with ObjectAssertExtensions.ShouldEqual
    // When called on arrays, C# chooses the Object version which compares references, not elements.
    // This is a known limitation of having multiple extension methods with the same name.
    // To properly compare collections, use AssertThatCollection.AreEqual directly or cast to IEnumerable<T>

    [Fact]
    public void ShouldEqual_WithList_WorksCorrectly()
    {
        // Arrange - Using List<T> explicitly to get the right extension method
        var actual = new List<int> { 1, 2, 3 };
        IEnumerable<int> expected = new[] { 1, 2, 3 };

        // Act - This works because List<T> to IEnumerable<T> gets the collection extension
        var result = actual.ShouldEqual(expected, "Should be equal");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldEqual_WithDifferentCollections_Throws()
    {
        // Arrange - Using explicit IEnumerable<T> to get the right extension method
        IEnumerable<int> actual = new[] { 1, 2, 3 };
        IEnumerable<int> expected = new[] { 1, 2, 4 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldEqual(expected, "Should be equal"));

        Assert.Contains("Should be equal", ex.Message);
    }

    [Fact]
    public void ShouldEqual_WithDifferentOrder_Throws()
    {
        // Arrange - Using List to avoid the extension method conflict
        var actual = new List<int> { 1, 2, 3 };
        var expected = new List<int> { 3, 2, 1 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldEqual(expected, "Order matters"));

        Assert.Contains("Order matters", ex.Message);
    }

    #endregion

    #region ShouldBeSubsetOf Tests

    [Fact]
    public void ShouldBeSubsetOf_WhenSubset_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 1, 2 };
        var superset = new[] { 1, 2, 3, 4 };

        // Act
        var result = actual.ShouldBeSubsetOf(superset, "Should be subset");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldBeSubsetOf_WhenNotSubset_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 5 };
        var superset = new[] { 1, 2, 3, 4 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeSubsetOf(superset, "Should be subset"));

        Assert.Contains("Should be subset", ex.Message);
    }

    #endregion

    #region ShouldBeSupersetOf Tests

    [Fact]
    public void ShouldBeSupersetOf_WhenSuperset_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 1, 2, 3, 4 };
        var subset = new[] { 2, 3 };

        // Act
        var result = actual.ShouldBeSupersetOf(subset, "Should be superset");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldBeSupersetOf_WhenNotSuperset_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2 };
        var subset = new[] { 2, 3 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeSupersetOf(subset, "Should be superset"));

        Assert.Contains("Should be superset", ex.Message);
    }

    #endregion

    #region ShouldHaveUniqueElements Tests

    [Fact]
    public void ShouldHaveUniqueElements_WhenAllUnique_ReturnsCollection()
    {
        // Arrange
        var actual = new[] { 1, 2, 3, 4 };

        // Act
        var result = actual.ShouldHaveUniqueElements("Should be unique");

        // Assert
        Assert.Same(actual, result);
    }

    [Fact]
    public void ShouldHaveUniqueElements_WithDuplicates_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 2, 3 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldHaveUniqueElements("Should be unique"));

        Assert.Contains("Should be unique", ex.Message);
    }

    #endregion

    #region ShouldHaveSingleElement Tests

    [Fact]
    public void ShouldHaveSingleElement_WithSingleElement_ReturnsElement()
    {
        // Arrange
        var actual = new[] { 42 };

        // Act
        var result = actual.ShouldHaveSingleElement("Should have single element");

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void ShouldHaveSingleElement_WithMultipleElements_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 3 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldHaveSingleElement("Should have single"));

        Assert.Contains("Should have single", ex.Message);
    }

    [Fact]
    public void ShouldHaveSingleElement_WithEmpty_Throws()
    {
        // Arrange
        var actual = new int[] { };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldHaveSingleElement("Should have single"));

        Assert.Contains("Should have single", ex.Message);
    }

    [Fact]
    public void ShouldHaveSingleElement_WithPredicate_ReturnsMatchingElement()
    {
        // Arrange
        var actual = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = actual.ShouldHaveSingleElement(x => x > 4, "Single element > 4");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void ShouldHaveSingleElement_WithPredicate_MultipleMatches_Throws()
    {
        // Arrange
        var actual = new[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldHaveSingleElement(x => x > 2, "Multiple matches"));

        Assert.Contains("Multiple matches", ex.Message);
    }

    #endregion

    #region Fluent Chaining Tests

    [Fact]
    public void CollectionExtensions_CanBeChained()
    {
        // This demonstrates fluent chaining
        var result = new[] { 1, 2, 3, 4, 5 }
            .ShouldNotBeEmpty("Not empty")
            .ShouldHaveCount(5, "Has 5 items")
            .ShouldContain(3, "Contains 3")
            .ShouldNotContain(10, "Doesn't contain 10")
            .ShouldHaveUniqueElements("All unique")
            .ShouldAllMatch(x => x > 0, "All positive");

        Assert.NotNull(result);
    }

    #endregion
}