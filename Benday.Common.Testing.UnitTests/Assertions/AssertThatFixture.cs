using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Assertions;

public class AssertThatFixture : TestClassBase
{
    public AssertThatFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region IsTrue Tests

    [Fact]
    public void IsTrue_WithTrueCondition_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.IsTrue(true, "Should not throw");
        AssertThat.IsTrue(1 == 1, "Math works");
        AssertThat.IsTrue(true); // Without message
    }

    [Fact]
    public void IsTrue_WithFalseCondition_ThrowsWithMessage()
    {
        // Arrange
        var customMessage = "Expected condition to be true";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsTrue(false, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Assert.IsTrue failed", ex.Message);
    }

    [Fact]
    public void IsTrue_WithFalseConditionNoMessage_ThrowsWithDefaultMessage()
    {
        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsTrue(false));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.IsTrue failed", ex.Message);
        Assert.Contains("Expected: True", ex.Message);
    }

    #endregion

    #region IsFalse Tests

    [Fact]
    public void IsFalse_WithFalseCondition_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.IsFalse(false, "Should not throw");
        AssertThat.IsFalse(1 == 2, "Math works");
        AssertThat.IsFalse(false); // Without message
    }

    [Fact]
    public void IsFalse_WithTrueCondition_ThrowsWithMessage()
    {
        // Arrange
        var customMessage = "Expected condition to be false";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsFalse(true, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Assert.IsFalse failed", ex.Message);
    }

    [Fact]
    public void IsFalse_WithTrueConditionNoMessage_ThrowsWithDefaultMessage()
    {
        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsFalse(true));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.IsFalse failed", ex.Message);
        Assert.Contains("Expected: False", ex.Message);
    }

    #endregion

    #region AreEqual Tests

    [Fact]
    public void AreEqual_WithEqualValues_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.AreEqual(5, 5, "Numbers are equal");
        AssertThat.AreEqual("test", "test", "Strings are equal");
        AssertThat.AreEqual(5, 5); // Without message
        AssertThat.AreEqual<object?>(null, null, "Both null");
    }

    [Fact]
    public void AreEqual_WithDifferentValues_ThrowsWithMessage()
    {
        // Arrange
        var expected = 10;
        var actual = 20;
        var customMessage = "Values should be equal";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreEqual(expected, actual, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Expected: 10", ex.Message);
        Assert.Contains("Actual: 20", ex.Message);
    }

    [Fact]
    public void AreEqual_WithDifferentValuesNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        var expected = "hello";
        var actual = "world";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreEqual(expected, actual));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.AreEqual failed", ex.Message);
        Assert.Contains("Expected: 'hello'", ex.Message);
        Assert.Contains("Actual: 'world'", ex.Message);
    }

    #endregion

    #region AreNotEqual Tests

    [Fact]
    public void AreNotEqual_WithDifferentValues_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.AreNotEqual(5, 10, "Numbers are different");
        AssertThat.AreNotEqual("test", "different", "Strings are different");
        AssertThat.AreNotEqual(5, 10); // Without message
        AssertThat.AreNotEqual<string?>("test", null, "One is null");
    }

    [Fact]
    public void AreNotEqual_WithEqualValues_ThrowsWithMessage()
    {
        // Arrange
        var value = 42;
        var customMessage = "Values should not be equal";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreNotEqual(value, value, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("42", ex.Message);
    }

    [Fact]
    public void AreNotEqual_WithEqualValuesNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        var value = "same";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreNotEqual(value, value));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.AreNotEqual failed", ex.Message);
        Assert.Contains("'same'", ex.Message);
    }

    #endregion

    #region IsNull Tests

    [Fact]
    public void IsNull_WithNullValue_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        string? nullString = null;
        object? nullObject = null;

        AssertThat.IsNull(nullString, "String is null");
        AssertThat.IsNull(nullObject, "Object is null");
        AssertThat.IsNull(nullString); // Without message
    }

    [Fact]
    public void IsNull_WithNonNullValue_ThrowsWithMessage()
    {
        // Arrange
        var value = "not null";
        var customMessage = "Expected null value";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsNull(value, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Expected: <null>", ex.Message);
        Assert.Contains("Actual: 'not null'", ex.Message);
    }

    [Fact]
    public void IsNull_WithNonNullValueNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        var value = new object();

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsNull(value));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.IsNull failed", ex.Message);
        Assert.Contains("Expected: <null>", ex.Message);
    }

    #endregion

    #region IsNotNull Tests

    [Fact]
    public void IsNotNull_WithNonNullValue_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.IsNotNull("value", "String is not null");
        AssertThat.IsNotNull(new object(), "Object is not null");
        AssertThat.IsNotNull("value"); // Without message
    }

    [Fact]
    public void IsNotNull_WithNullValue_ThrowsWithMessage()
    {
        // Arrange
        string? nullValue = null;
        var customMessage = "Expected non-null value";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsNotNull(nullValue, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Expected: '<not null>'", ex.Message);
        Assert.Contains("Actual: <null>", ex.Message);
    }

    [Fact]
    public void IsNotNull_WithNullValueNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        object? nullValue = null;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsNotNull(nullValue));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.IsNotNull failed", ex.Message);
        Assert.Contains("Expected: '<not null>'", ex.Message);
    }

    #endregion

    #region IsOfType Tests

    [Fact]
    public void IsOfType_WithCorrectType_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.IsOfType<string>("test", "Is string");
        AssertThat.IsOfType<int>(42, "Is int");
        AssertThat.IsOfType<object>(new object(), "Is object");
        AssertThat.IsOfType<string>("test"); // Without message
    }

    [Fact]
    public void IsOfType_WithWrongType_ThrowsWithMessage()
    {
        // Arrange
        object value = 123;
        var customMessage = "Expected string type";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsOfType<string>(value, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Expected type: String", ex.Message);
        Assert.Contains("Actual type: Int32", ex.Message);
    }

    [Fact]
    public void IsOfType_WithWrongTypeNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        object value = "text";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsOfType<int>(value));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.IsOfType failed", ex.Message);
        Assert.Contains("Expected type: Int32", ex.Message);
        Assert.Contains("Actual type: String", ex.Message);
    }

    [Fact]
    public void IsOfType_WithNullValue_ThrowsWithMessage()
    {
        // Arrange
        object? nullValue = null;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsOfType<string>(nullValue, "Cannot check type of null"));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("value was null", ex.Message);
    }

    #endregion

    #region IsNotOfType Tests

    [Fact]
    public void IsNotOfType_WithDifferentType_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.IsNotOfType<int>("test", "Not an int");
        AssertThat.IsNotOfType<string>(42, "Not a string");
        AssertThat.IsNotOfType<int>("test"); // Without message
        AssertThat.IsNotOfType<string>(null, "Null is not string");
    }

    [Fact]
    public void IsNotOfType_WithSameType_ThrowsWithMessage()
    {
        // Arrange
        object value = "test";
        var customMessage = "Should not be string";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsNotOfType<string>(value, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Value should not be of type: String", ex.Message);
    }

    [Fact]
    public void IsNotOfType_WithSameTypeNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        object value = 42;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.IsNotOfType<int>(value));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.IsNotOfType failed", ex.Message);
        Assert.Contains("Value should not be of type: Int32", ex.Message);
    }

    #endregion

    #region Throws Tests

    [Fact]
    public void Throws_WithExpectedException_ReturnsException()
    {
        // Act
        var ex = AssertThat.Throws<InvalidOperationException>(() =>
            throw new InvalidOperationException("Test exception"), "Should throw");

        var exNoMessage = AssertThat.Throws<InvalidOperationException>(() =>
            throw new InvalidOperationException("Test exception"));

        // Assert
        Assert.NotNull(ex);
        Assert.Equal("Test exception", ex.Message);
        Assert.NotNull(exNoMessage);
    }

    [Fact]
    public void Throws_WithNoException_ThrowsWithMessage()
    {
        // Arrange
        var customMessage = "Expected exception";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.Throws<InvalidOperationException>(() => { }, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("no exception was thrown", ex.Message);
    }

    [Fact]
    public void Throws_WithWrongExceptionType_ThrowsWithMessage()
    {
        // Arrange
        var customMessage = "Wrong exception type";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.Throws<InvalidOperationException>(
                () => throw new ArgumentException("Different exception"),
                customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Expected exception type: InvalidOperationException", ex.Message);
        Assert.Contains("Actual exception type: ArgumentException", ex.Message);
    }

    [Fact]
    public void Throws_WithNullAction_ThrowsArgumentNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            AssertThat.Throws<Exception>(null!, "message"));
    }

    #endregion

    #region DoesNotThrow Tests

    [Fact]
    public void DoesNotThrow_WithNoException_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        AssertThat.DoesNotThrow(() => { _ = 1 + 1; }, "No exception");
        AssertThat.DoesNotThrow(() => { _ = "test"; }); // Without message
    }

    [Fact]
    public void DoesNotThrow_WithException_ThrowsWithMessage()
    {
        // Arrange
        var customMessage = "Should not throw";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.DoesNotThrow(
                () => throw new InvalidOperationException("Oops"),
                customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Expected no exception", ex.Message);
        Assert.Contains("InvalidOperationException was thrown", ex.Message);
    }

    [Fact]
    public void DoesNotThrow_WithExceptionNoMessage_ThrowsWithDefaultMessage()
    {
        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.DoesNotThrow(() => throw new ArgumentException("Error")));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.DoesNotThrow failed", ex.Message);
        Assert.Contains("ArgumentException was thrown", ex.Message);
    }

    #endregion

    #region AreSame Tests

    [Fact]
    public void AreSame_WithSameReference_DoesNotThrow()
    {
        // Arrange
        var obj = new object();
        var sameRef = obj;

        // Act & Assert - Should not throw
        AssertThat.AreSame(obj, sameRef, "Same reference");
        AssertThat.AreSame(obj, sameRef); // Without message
        AssertThat.AreSame(null, null, "Both null");
    }

    [Fact]
    public void AreSame_WithDifferentReferences_ThrowsWithMessage()
    {
        // Arrange
        var obj1 = new object();
        var obj2 = new object();
        var customMessage = "Should be same reference";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreSame(obj1, obj2, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Assert.AreSame failed", ex.Message);
    }

    [Fact]
    public void AreSame_WithDifferentReferencesNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        var obj1 = "test1";
        var obj2 = "test2";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreSame(obj1, obj2));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.AreSame failed", ex.Message);
    }

    #endregion

    #region AreNotSame Tests

    [Fact]
    public void AreNotSame_WithDifferentReferences_DoesNotThrow()
    {
        // Arrange
        var obj1 = new object();
        var obj2 = new object();

        // Act & Assert - Should not throw
        AssertThat.AreNotSame(obj1, obj2, "Different references");
        AssertThat.AreNotSame(obj1, obj2); // Without message
        AssertThat.AreNotSame(obj1, null, "One is null");
    }

    [Fact]
    public void AreNotSame_WithSameReference_ThrowsWithMessage()
    {
        // Arrange
        var obj = new object();
        var customMessage = "Should not be same reference";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreNotSame(obj, obj, customMessage));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains(customMessage, ex.Message);
        Assert.Contains("Assert.AreNotSame failed", ex.Message);
    }

    [Fact]
    public void AreNotSame_WithSameReferenceNoMessage_ThrowsWithDefaultMessage()
    {
        // Arrange
        var obj = "test";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AreNotSame(obj, obj));

        WriteLine($"Exception message: {ex.Message}");
        Assert.Contains("Assert.AreNotSame failed", ex.Message);
    }

    #endregion

    #region Fail Tests

    [Fact]
    public void Fail_AlwaysThrows()
    {
        // Arrange
        var message = "This test failed";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.Fail(message));

        Assert.Equal(message, ex.Message);
    }

    #endregion
}