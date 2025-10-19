using System;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Extensions;

public class StringAssertExtensionsFixture : TestClassBase
{
    public StringAssertExtensionsFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region ShouldBeNullOrEmpty Tests

    [Fact]
    public void ShouldBeNullOrEmpty_WithNull_ReturnsNull()
    {
        // Arrange
        string? actual = null;

        // Act
        var result = actual.ShouldBeNullOrEmpty("Should be null or empty");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ShouldBeNullOrEmpty_WithEmpty_ReturnsEmpty()
    {
        // Arrange
        var actual = "";

        // Act
        var result = actual.ShouldBeNullOrEmpty("Should be null or empty");

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void ShouldBeNullOrEmpty_WithNonEmpty_Throws()
    {
        // Arrange
        var actual = "not empty";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeNullOrEmpty("Should be null or empty"));

        Assert.Contains("Should be null or empty", ex.Message);
    }

    #endregion

    #region ShouldNotBeNullOrEmpty Tests

    [Fact]
    public void ShouldNotBeNullOrEmpty_WithValue_ReturnsValue()
    {
        // Arrange
        var actual = "test";

        // Act
        var result = actual.ShouldNotBeNullOrEmpty("Should have value");

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void ShouldNotBeNullOrEmpty_WithNull_Throws()
    {
        // Arrange
        string? actual = null;

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeNullOrEmpty("Should not be null"));

        Assert.Contains("Should not be null", ex.Message);
    }

    [Fact]
    public void ShouldNotBeNullOrEmpty_WithEmpty_Throws()
    {
        // Arrange
        var actual = "";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeNullOrEmpty("Should not be empty"));

        Assert.Contains("Should not be empty", ex.Message);
    }

    #endregion

    #region ShouldBeNullOrWhiteSpace Tests

    [Fact]
    public void ShouldBeNullOrWhiteSpace_WithWhitespace_ReturnsValue()
    {
        // Arrange
        var actual = "   ";

        // Act
        var result = actual.ShouldBeNullOrWhiteSpace("Should be whitespace");

        // Assert
        Assert.Equal("   ", result);
    }

    [Fact]
    public void ShouldBeNullOrWhiteSpace_WithContent_Throws()
    {
        // Arrange
        var actual = " text ";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldBeNullOrWhiteSpace("Should be whitespace"));

        Assert.Contains("Should be whitespace", ex.Message);
    }

    #endregion

    #region ShouldNotBeNullOrWhiteSpace Tests

    [Fact]
    public void ShouldNotBeNullOrWhiteSpace_WithContent_ReturnsValue()
    {
        // Arrange
        var actual = "content";

        // Act
        var result = actual.ShouldNotBeNullOrWhiteSpace("Should have content");

        // Assert
        Assert.Equal("content", result);
    }

    [Fact]
    public void ShouldNotBeNullOrWhiteSpace_WithWhitespace_Throws()
    {
        // Arrange
        var actual = "   ";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotBeNullOrWhiteSpace("Should have content"));

        Assert.Contains("Should have content", ex.Message);
    }

    #endregion

    #region ShouldStartWith Tests

    [Fact]
    public void ShouldStartWith_WithCorrectPrefix_ReturnsValue()
    {
        // Arrange
        var actual = "Hello World";

        // Act
        var result = actual.ShouldStartWith("Hello", "Should start with Hello");

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void ShouldStartWith_WithWrongPrefix_Throws()
    {
        // Arrange
        var actual = "Hello World";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldStartWith("Goodbye", "Wrong prefix"));

        Assert.Contains("Wrong prefix", ex.Message);
    }

    [Fact]
    public void ShouldStartWith_WithComparison_CaseInsensitive_Works()
    {
        // Arrange
        var actual = "hello world";

        // Act
        var result = actual.ShouldStartWith("HELLO", StringComparison.OrdinalIgnoreCase, "Case insensitive");

        // Assert
        Assert.Equal("hello world", result);
    }

    #endregion

    #region ShouldEndWith Tests

    [Fact]
    public void ShouldEndWith_WithCorrectSuffix_ReturnsValue()
    {
        // Arrange
        var actual = "Hello World";

        // Act
        var result = actual.ShouldEndWith("World", "Should end with World");

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void ShouldEndWith_WithWrongSuffix_Throws()
    {
        // Arrange
        var actual = "Hello World";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldEndWith("Earth", "Wrong suffix"));

        Assert.Contains("Wrong suffix", ex.Message);
    }

    [Fact]
    public void ShouldEndWith_WithComparison_CaseInsensitive_Works()
    {
        // Arrange
        var actual = "hello world";

        // Act
        var result = actual.ShouldEndWith("WORLD", StringComparison.OrdinalIgnoreCase, "Case insensitive");

        // Assert
        Assert.Equal("hello world", result);
    }

    #endregion

    #region ShouldContain Tests

    [Fact]
    public void ShouldContain_WithSubstring_ReturnsValue()
    {
        // Arrange
        var actual = "The quick brown fox";

        // Act
        var result = actual.ShouldContain("quick", "Should contain quick");

        // Assert
        Assert.Equal("The quick brown fox", result);
    }

    [Fact]
    public void ShouldContain_WithMissingSubstring_Throws()
    {
        // Arrange
        var actual = "The quick brown fox";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldContain("lazy", "Missing substring"));

        Assert.Contains("Missing substring", ex.Message);
    }

    [Fact]
    public void ShouldContain_WithComparison_CaseInsensitive_Works()
    {
        // Arrange
        var actual = "The quick brown fox";

        // Act
        var result = actual.ShouldContain("QUICK", StringComparison.OrdinalIgnoreCase, "Case insensitive");

        // Assert
        Assert.Equal("The quick brown fox", result);
    }

    #endregion

    #region ShouldNotContain Tests

    [Fact]
    public void ShouldNotContain_WithAbsentSubstring_ReturnsValue()
    {
        // Arrange
        var actual = "The quick brown fox";

        // Act
        var result = actual.ShouldNotContain("lazy", "Should not contain lazy");

        // Assert
        Assert.Equal("The quick brown fox", result);
    }

    [Fact]
    public void ShouldNotContain_WithPresentSubstring_Throws()
    {
        // Arrange
        var actual = "The quick brown fox";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotContain("quick", "Should not contain"));

        Assert.Contains("Should not contain", ex.Message);
    }

    #endregion

    #region ShouldMatch Tests

    [Fact]
    public void ShouldMatch_WithMatchingPattern_ReturnsValue()
    {
        // Arrange
        var actual = "123-456-7890";

        // Act
        var result = actual.ShouldMatch(@"\d{3}-\d{3}-\d{4}", "Phone pattern");

        // Assert
        Assert.Equal("123-456-7890", result);
    }

    [Fact]
    public void ShouldMatch_WithNonMatchingPattern_Throws()
    {
        // Arrange
        var actual = "not a phone";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldMatch(@"\d{3}-\d{3}-\d{4}", "Phone pattern"));

        Assert.Contains("Phone pattern", ex.Message);
    }

    [Fact]
    public void ShouldMatch_WithRegexOptions_Works()
    {
        // Arrange
        var actual = "HELLO";

        // Act
        var result = actual.ShouldMatch("hello", RegexOptions.IgnoreCase, "Case insensitive");

        // Assert
        Assert.Equal("HELLO", result);
    }

    #endregion

    #region ShouldNotMatch Tests

    [Fact]
    public void ShouldNotMatch_WithNonMatchingPattern_ReturnsValue()
    {
        // Arrange
        var actual = "not a phone";

        // Act
        var result = actual.ShouldNotMatch(@"\d{3}-\d{3}-\d{4}", "Not phone pattern");

        // Assert
        Assert.Equal("not a phone", result);
    }

    [Fact]
    public void ShouldNotMatch_WithMatchingPattern_Throws()
    {
        // Arrange
        var actual = "123-456-7890";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotMatch(@"\d{3}-\d{3}-\d{4}", "Should not match"));

        Assert.Contains("Should not match", ex.Message);
    }

    #endregion

    #region ShouldHaveLength Tests

    [Fact]
    public void ShouldHaveLength_WithCorrectLength_ReturnsValue()
    {
        // Arrange
        var actual = "test";

        // Act
        var result = actual.ShouldHaveLength(4, "Length is 4");

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void ShouldHaveLength_WithWrongLength_Throws()
    {
        // Arrange
        var actual = "test";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldHaveLength(5, "Wrong length"));

        Assert.Contains("Wrong length", ex.Message);
    }

    #endregion

    #region ShouldEqualIgnoreCase Tests

    [Fact]
    public void ShouldEqualIgnoreCase_WithEqualStrings_ReturnsValue()
    {
        // Arrange
        var actual = "hello";

        // Act
        var result = actual.ShouldEqualIgnoreCase("HELLO", "Case insensitive equal");

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ShouldEqualIgnoreCase_WithDifferentStrings_Throws()
    {
        // Arrange
        var actual = "hello";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldEqualIgnoreCase("GOODBYE", "Should equal"));

        Assert.Contains("Should equal", ex.Message);
    }

    #endregion

    #region ShouldNotEqualIgnoreCase Tests

    [Fact]
    public void ShouldNotEqualIgnoreCase_WithDifferentStrings_ReturnsValue()
    {
        // Arrange
        var actual = "hello";

        // Act
        var result = actual.ShouldNotEqualIgnoreCase("GOODBYE", "Not equal");

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ShouldNotEqualIgnoreCase_WithEqualStrings_Throws()
    {
        // Arrange
        var actual = "hello";

        // Act & Assert
        var ex = Assert.Throws<AssertionException>(() =>
            actual.ShouldNotEqualIgnoreCase("HELLO", "Should not equal"));

        Assert.Contains("Should not equal", ex.Message);
    }

    #endregion

    #region Fluent Chaining Tests

    [Fact]
    public void StringExtensions_CanBeChained()
    {
        // This demonstrates fluent chaining
        var result = "Hello World"
            .ShouldNotBeNullOrEmpty("Not empty")
            .ShouldStartWith("Hello", "Starts with Hello")
            .ShouldEndWith("World", "Ends with World")
            .ShouldContain("lo Wo", "Contains substring")
            .ShouldHaveLength(11, "Correct length");

        Assert.Equal("Hello World", result);
    }

    #endregion
}