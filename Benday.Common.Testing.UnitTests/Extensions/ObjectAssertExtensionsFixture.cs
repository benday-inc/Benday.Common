using System;
using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Extensions;

public class ObjectAssertExtensionsFixture : TestClassBase
{
    public ObjectAssertExtensionsFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region ShouldEqual Tests

    [Fact]
    public void ShouldEqual_WithEqualValues_ReturnsActual()
    {
        // Arrange
        var actual = 42;
        var expected = 42;

        // Act
        var result = actual.ShouldEqual(expected, "Values are equal");

        // Assert
        Assert.Equal(actual, result);
    }

    [Fact]
    public void ShouldEqual_WithDifferentValues_Throws()
    {
        // Arrange
        var actual = "hello";
        var expected = "world";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldEqual(expected, "Should be equal"));

        WriteLine($"Exception: {ex.Message}");
        Assert.Contains("Should be equal", ex.Message);
    }

    #endregion

    #region ShouldNotEqual Tests

    [Fact]
    public void ShouldNotEqual_WithDifferentValues_ReturnsActual()
    {
        // Arrange
        var actual = "test";
        var notExpected = "different";

        // Act
        var result = actual.ShouldNotEqual(notExpected, "Should not be equal");

        // Assert
        Assert.Equal(actual, result);
    }

    [Fact]
    public void ShouldNotEqual_WithEqualValues_Throws()
    {
        // Arrange
        var value = 123;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            value.ShouldNotEqual(123, "Should not be equal"));

        Assert.Contains("Should not be equal", ex.Message);
    }

    #endregion

    #region ShouldBeNull Tests

    [Fact]
    public void ShouldBeNull_WithNull_ReturnsNull()
    {
        // Arrange
        string? actual = null;

        // Act
        var result = actual.ShouldBeNull("Should be null");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ShouldBeNull_WithNonNull_Throws()
    {
        // Arrange
        var actual = "not null";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeNull("Should be null"));

        Assert.Contains("Should be null", ex.Message);
    }

    #endregion

    #region ShouldNotBeNull Tests

    [Fact]
    public void ShouldNotBeNull_WithNonNull_ReturnsActual()
    {
        // Arrange
        var actual = "value";

        // Act
        var result = actual.ShouldNotBeNull("Should not be null");

        // Assert
        Assert.Equal(actual, result);
    }

    [Fact]
    public void ShouldNotBeNull_WithNull_Throws()
    {
        // Arrange
        string? actual = null;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeNull("Should not be null"));

        Assert.Contains("Should not be null", ex.Message);
    }

    #endregion

    #region ShouldBeOfType Tests

    [Fact]
    public void ShouldBeOfType_WithCorrectType_ReturnsCastedValue()
    {
        // Arrange
        object actual = "test string";

        // Act
        var result = actual.ShouldBeOfType<string>("Should be string");

        // Assert
        Assert.Equal("test string", result);
        Assert.IsType<string>(result);
    }

    [Fact]
    public void ShouldBeOfType_WithWrongType_Throws()
    {
        // Arrange
        object actual = 123;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeOfType<string>("Should be string"));

        Assert.Contains("Should be string", ex.Message);
    }

    [Fact]
    public void ShouldBeOfType_WithNull_Throws()
    {
        // Arrange
        object? actual = null;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeOfType<string>("Should be string"));

        Assert.Contains("value was null", ex.Message);
    }

    #endregion

    #region ShouldNotBeOfType Tests

    [Fact]
    public void ShouldNotBeOfType_WithDifferentType_ReturnsActual()
    {
        // Arrange
        object actual = 123;

        // Act
        var result = actual.ShouldNotBeOfType<string>("Should not be string");

        // Assert
        Assert.Equal(actual, result);
    }

    [Fact]
    public void ShouldNotBeOfType_WithSameType_Throws()
    {
        // Arrange
        object actual = "test";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeOfType<string>("Should not be string"));

        Assert.Contains("Should not be string", ex.Message);
    }

    #endregion

    #region ShouldBeSameAs Tests

    [Fact]
    public void ShouldBeSameAs_WithSameReference_ReturnsActual()
    {
        // Arrange
        var obj = new object();
        var sameRef = obj;

        // Act
        var result = obj.ShouldBeSameAs(sameRef, "Same reference");

        // Assert
        Assert.Same(obj, result);
    }

    [Fact]
    public void ShouldBeSameAs_WithDifferentReferences_Throws()
    {
        // Arrange
        var obj1 = new object();
        var obj2 = new object();

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            obj1.ShouldBeSameAs(obj2, "Should be same"));

        Assert.Contains("Should be same", ex.Message);
    }

    #endregion

    #region ShouldNotBeSameAs Tests

    [Fact]
    public void ShouldNotBeSameAs_WithDifferentReferences_ReturnsActual()
    {
        // Arrange
        var obj1 = new object();
        var obj2 = new object();

        // Act
        var result = obj1.ShouldNotBeSameAs(obj2, "Not same reference");

        // Assert
        Assert.Equal(obj1, result);
    }

    [Fact]
    public void ShouldNotBeSameAs_WithSameReference_Throws()
    {
        // Arrange
        var obj = new object();

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            obj.ShouldNotBeSameAs(obj, "Should not be same"));

        Assert.Contains("Should not be same", ex.Message);
    }

    #endregion

    #region ShouldThrow Tests

    [Fact]
    public void ShouldThrow_WithExpectedException_ReturnsException()
    {
        // Arrange
        Action action = () => throw new InvalidOperationException("Test");

        // Act
        var ex = action.ShouldThrow<InvalidOperationException>("Should throw");

        // Assert
        Assert.NotNull(ex);
        Assert.Equal("Test", ex.Message);
    }

    [Fact]
    public void ShouldThrow_WithNoException_Throws()
    {
        // Arrange
        Action action = () => { _ = 1 + 1; };

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            action.ShouldThrow<InvalidOperationException>("Should throw"));

        Assert.Contains("Should throw", ex.Message);
    }

    [Fact]
    public void ShouldThrow_WithWrongException_Throws()
    {
        // Arrange
        Action action = () => throw new ArgumentException("Wrong");

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            action.ShouldThrow<InvalidOperationException>("Should throw InvalidOp"));

        Assert.Contains("Should throw InvalidOp", ex.Message);
    }

    #endregion

    #region ShouldNotThrow Tests

    [Fact]
    public void ShouldNotThrow_WithNoException_ReturnsAction()
    {
        // Arrange
        Action action = () => { _ = 1 + 1; };

        // Act
        var result = action.ShouldNotThrow("Should not throw");

        // Assert
        Assert.Same(action, result);
    }

    [Fact]
    public void ShouldNotThrow_WithException_Throws()
    {
        // Arrange
        Action action = () => throw new Exception("Oops");

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            action.ShouldNotThrow("Should not throw"));

        Assert.Contains("Should not throw", ex.Message);
    }

    #endregion

    #region ShouldBeTrue Tests

    [Fact]
    public void ShouldBeTrue_WithTrue_ReturnsTrue()
    {
        // Arrange
        var actual = true;

        // Act
        var result = actual.ShouldBeTrue("Should be true");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldBeTrue_WithFalse_Throws()
    {
        // Arrange
        var actual = false;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeTrue("Should be true"));

        Assert.Contains("Should be true", ex.Message);
    }

    #endregion

    #region ShouldBeFalse Tests

    [Fact]
    public void ShouldBeFalse_WithFalse_ReturnsFalse()
    {
        // Arrange
        var actual = false;

        // Act
        var result = actual.ShouldBeFalse("Should be false");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShouldBeFalse_WithTrue_Throws()
    {
        // Arrange
        var actual = true;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeFalse("Should be false"));

        Assert.Contains("Should be false", ex.Message);
    }

    #endregion

    #region Fluent Chaining Tests

    [Fact]
    public void FluentMethods_CanBeChained()
    {
        // This demonstrates that fluent methods return the value and can be chained
        var result = "test"
            .ShouldNotBeNull("Not null")
            .ShouldEqual("test", "Equals test")
            .ShouldBeOfType<string>("Is string");

        Assert.Equal("test", result);
    }

    #endregion
}