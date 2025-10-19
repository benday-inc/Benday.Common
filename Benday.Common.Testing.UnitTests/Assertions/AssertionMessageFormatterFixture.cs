using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Assertions;

public class AssertionMessageFormatterFixture : TestClassBase
{
    public AssertionMessageFormatterFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region FormatComparisonMessage Tests

    [Fact]
    public void FormatComparisonMessage_WithUserMessage_IncludesUserMessageInOutput()
    {
        // Arrange
        var expected = "expected value";
        var actual = "actual value";
        var userMessage = "This is a custom message";
        var operation = "AreEqual";

        // Act
        var result = AssertionMessageFormatter.FormatComparisonMessage(expected, actual, userMessage, operation);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains(userMessage, result);
        Assert.Contains($"Assert.{operation} failed", result);
        Assert.Contains($"Expected: '{expected}'", result);
        Assert.Contains($"Actual: '{actual}'", result);
    }

    [Fact]
    public void FormatComparisonMessage_WithoutUserMessage_ProducesDefaultMessage()
    {
        // Arrange
        var expected = 42;
        var actual = 24;
        var operation = "AreEqual";

        // Act
        var result = AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, operation);

        // Assert
        WriteLine($"Result: {result}");
        Assert.StartsWith($"Assert.{operation} failed", result);
        Assert.Contains($"Expected: {expected}", result);
        Assert.Contains($"Actual: {actual}", result);
    }

    [Fact]
    public void FormatComparisonMessage_WithNullValues_HandlesCorrectly()
    {
        // Arrange
        string? expected = null;
        string? actual = "not null";
        var operation = "AreEqual";

        // Act
        var result = AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, operation);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("Expected: <null>", result);
        Assert.Contains("Actual: 'not null'", result);
    }

    [Fact]
    public void FormatComparisonMessage_WithCollections_FormatsCorrectly()
    {
        // Arrange
        var expected = new[] { 1, 2, 3 };
        var actual = new[] { 4, 5, 6 };
        var operation = "AreEqual";

        // Act
        var result = AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, operation);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("Expected: [1, 2, 3] (Count: 3)", result);
        Assert.Contains("Actual: [4, 5, 6] (Count: 3)", result);
    }

    #endregion

    #region FormatSimpleMessage Tests

    [Fact]
    public void FormatSimpleMessage_WithUserMessage_IncludesUserMessage()
    {
        // Arrange
        var userMessage = "Custom error message";
        var operation = "IsTrue";
        var additionalInfo = "Expected: True";

        // Act
        var result = AssertionMessageFormatter.FormatSimpleMessage(userMessage, operation, additionalInfo);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains(userMessage, result);
        Assert.Contains($"Assert.{operation} failed", result);
        Assert.Contains(additionalInfo, result);
    }

    [Fact]
    public void FormatSimpleMessage_WithoutUserMessage_ProducesDefaultMessage()
    {
        // Arrange
        var operation = "IsFalse";
        var additionalInfo = "Expected: False";

        // Act
        var result = AssertionMessageFormatter.FormatSimpleMessage(string.Empty, operation, additionalInfo);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains($"Assert.{operation} failed", result);
        Assert.Contains(additionalInfo, result);
    }

    [Fact]
    public void FormatSimpleMessage_WithoutAdditionalInfo_OmitsIt()
    {
        // Arrange
        var operation = "Fail";

        // Act
        var result = AssertionMessageFormatter.FormatSimpleMessage(string.Empty, operation, null);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Equal($"Assert.{operation} failed.", result);
    }

    #endregion

    #region FormatCollectionMessage Tests

    [Fact]
    public void FormatCollectionMessage_WithUserMessage_IncludesUserMessage()
    {
        // Arrange
        var collection = new[] { 1, 2, 3 };
        var userMessage = "Collection check failed";
        var operation = "IsEmpty";
        var additionalInfo = "Expected empty collection";

        // Act
        var result = AssertionMessageFormatter.FormatCollectionMessage(collection, userMessage, operation, additionalInfo);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains(userMessage, result);
        Assert.Contains($"CollectionAssert.{operation} failed", result);
        Assert.Contains("Collection: [1, 2, 3] (Count: 3)", result);
        Assert.Contains(additionalInfo, result);
    }

    [Fact]
    public void FormatCollectionMessage_WithNullCollection_HandlesCorrectly()
    {
        // Arrange
        IEnumerable<int>? collection = null;
        var operation = "IsNotNull";

        // Act
        var result = AssertionMessageFormatter.FormatCollectionMessage(collection, string.Empty, operation, null);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains($"CollectionAssert.{operation} failed", result);
        Assert.DoesNotContain("Collection:", result);
    }

    [Fact]
    public void FormatCollectionMessage_WithLargeCollection_TruncatesDisplay()
    {
        // Arrange
        var collection = Enumerable.Range(1, 20);
        var operation = "HasCount";

        // Act
        var result = AssertionMessageFormatter.FormatCollectionMessage(collection, string.Empty, operation, null);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("...", result);
        Assert.Contains("(Count: 10+)", result);
    }

    #endregion

    #region FormatValue Tests

    [Fact]
    public void FormatValue_WithNull_ReturnsNullRepresentation()
    {
        // Act
        var result = AssertionMessageFormatter.FormatValue(null);

        // Assert
        Assert.Equal("<null>", result);
    }

    [Fact]
    public void FormatValue_WithEmptyString_ReturnsEmptyStringRepresentation()
    {
        // Act
        var result = AssertionMessageFormatter.FormatValue(string.Empty);

        // Assert
        Assert.Equal("<empty string>", result);
    }

    [Fact]
    public void FormatValue_WithRegularString_ReturnsQuotedString()
    {
        // Arrange
        var value = "test string";

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("'test string'", result);
    }

    [Fact]
    public void FormatValue_WithLongString_TruncatesWithEllipsis()
    {
        // Arrange
        var value = new string('a', 150);

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        WriteLine($"Result: {result}");
        Assert.StartsWith("'", result);
        Assert.EndsWith("...'", result);
        Assert.True(result.Length < 110); // Max display length + quotes + ellipsis
    }

    [Fact]
    public void FormatValue_WithNumber_ReturnsStringRepresentation()
    {
        // Arrange
        var value = 42;

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void FormatValue_WithDateTime_ReturnsStringRepresentation()
    {
        // Arrange
        var value = new DateTime(2024, 1, 15, 10, 30, 45);

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("2024", result);
        Assert.Contains("15", result);
    }

    [Fact]
    public void FormatValue_WithArray_ReturnsFormattedCollection()
    {
        // Arrange
        var value = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("[1, 2, 3, 4, 5] (Count: 5)", result);
    }

    [Fact]
    public void FormatValue_WithList_ReturnsFormattedCollection()
    {
        // Arrange
        var value = new List<string> { "one", "two", "three" };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("['one', 'two', 'three'] (Count: 3)", result);
    }

    [Fact]
    public void FormatValue_WithDictionary_ReturnsFormattedCollection()
    {
        // Arrange
        var value = new Dictionary<string, int>
        {
            { "a", 1 },
            { "b", 2 }
        };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("[", result);
        Assert.Contains("]", result);
        Assert.Contains("(Count: 2)", result);
    }

    [Fact]
    public void FormatValue_WithLargeCollection_TruncatesItems()
    {
        // Arrange
        var value = Enumerable.Range(1, 20).ToArray();

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("...", result);
        Assert.Contains("(Count: 10+)", result);
        // Should show first 10 items then ellipsis
        Assert.Contains("1, 2, 3, 4, 5, 6, 7, 8, 9, 10, ...", result);
    }

    [Fact]
    public void FormatValue_WithCustomObject_UsesToStringMethod()
    {
        // Arrange
        var value = new CustomTestObject { Name = "Test", Value = 42 };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("CustomTestObject: Test (42)", result);
    }

    [Fact]
    public void FormatValue_WithObjectReturningNullFromToString_HandlesCorrectly()
    {
        // Arrange
        var value = new ObjectWithNullToString();

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("<null>", result);
    }

    #endregion

    #region GetTypeName Tests

    [Fact]
    public void GetTypeName_WithSimpleType_ReturnsTypeName()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(string));

        // Assert
        Assert.Equal("String", result);
    }

    [Fact]
    public void GetTypeName_WithArrayType_ReturnsArrayNotation()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(int[]));

        // Assert
        Assert.Equal("Int32[]", result);
    }

    [Fact]
    public void GetTypeName_WithGenericType_ReturnsGenericNotation()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(List<string>));

        // Assert
        Assert.Equal("List<String>", result);
    }

    [Fact]
    public void GetTypeName_WithNestedGenericType_ReturnsFullGenericNotation()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(Dictionary<string, List<int>>));

        // Assert
        WriteLine($"Result: {result}");
        Assert.Equal("Dictionary<String, List<Int32>>", result);
    }

    [Fact]
    public void GetTypeName_WithMultipleGenericArguments_ReturnsAllArguments()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(Tuple<int, string, bool>));

        // Assert
        Assert.Equal("Tuple<Int32, String, Boolean>", result);
    }

    [Fact]
    public void GetTypeName_WithCustomClass_ReturnsClassName()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(CustomTestObject));

        // Assert
        Assert.Equal("CustomTestObject", result);
    }

    [Fact]
    public void GetTypeName_WithNullableType_ReturnsNullableNotation()
    {
        // Act
        var result = AssertionMessageFormatter.GetTypeName(typeof(int?));

        // Assert
        WriteLine($"Result: {result}");
        Assert.Equal("Nullable<Int32>", result);
    }

    #endregion

    #region Edge Cases and Special Scenarios

    [Fact]
    public void FormatValue_WithCollectionOfNulls_HandlesCorrectly()
    {
        // Arrange
        var value = new object?[] { null, null, null };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("[<null>, <null>, <null>] (Count: 3)", result);
    }

    [Fact]
    public void FormatValue_WithMixedCollection_FormatsEachItemCorrectly()
    {
        // Arrange
        var value = new object?[] { 1, "test", null, true };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        Assert.Equal("[1, 'test', <null>, True] (Count: 4)", result);
    }

    [Fact]
    public void FormatValue_WithNestedCollections_FormatsCorrectly()
    {
        // Arrange
        var value = new[] { new[] { 1, 2 }, new[] { 3, 4 } };

        // Act
        var result = AssertionMessageFormatter.FormatValue(value);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("[[1, 2] (Count: 2), [3, 4] (Count: 2)] (Count: 2)", result);
    }

    [Fact]
    public void FormatComparisonMessage_WithVeryLongStrings_TruncatesBoth()
    {
        // Arrange
        var expected = new string('a', 200);
        var actual = new string('b', 200);
        var operation = "AreEqual";

        // Act
        var result = AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, operation);

        // Assert
        WriteLine($"Result: {result}");
        Assert.Contains("...'", result);
        // Check that both expected and actual values are truncated
        Assert.Contains("'aaaa", result); // Start of expected value
        Assert.Contains("'bbbb", result); // Start of actual value
        // The entire message should be reasonable length (not 400+ characters)
        Assert.True(result.Length < 260, $"Result length was {result.Length}, expected less than 260");
    }

    #endregion

    #region Helper Classes

    private class CustomTestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }

        public override string ToString()
        {
            return $"CustomTestObject: {Name} ({Value})";
        }
    }

    private class ObjectWithNullToString
    {
        public override string? ToString()
        {
            return null;
        }
    }

    #endregion
}