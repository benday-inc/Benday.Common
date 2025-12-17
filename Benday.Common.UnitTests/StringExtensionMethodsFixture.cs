using System;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class StringExtensionMethodsFixture : TestClassBase
{
    public StringExtensionMethodsFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region SafeToString Tests

    [Fact]
    public void SafeToString_ReturnsValue_WhenNotNull()
    {
        // arrange
        string? input = "hello";

        // act
        var actual = input.SafeToString();

        // assert
        actual.ShouldEqual("hello", "SafeToString should return the input value.");
    }

    [Fact]
    public void SafeToString_ReturnsEmptyString_WhenNull()
    {
        // arrange
        string? input = null;

        // act
        var actual = input.SafeToString();

        // assert
        actual.ShouldEqual(string.Empty, "SafeToString should return empty string for null.");
    }

    [Fact]
    public void SafeToString_ReturnsDefaultValue_WhenNullAndDefaultProvided()
    {
        // arrange
        string? input = null;
        var defaultValue = "default";

        // act
        var actual = input.SafeToString(defaultValue);

        // assert
        actual.ShouldEqual(defaultValue, "SafeToString should return the default value for null.");
    }

    [Fact]
    public void SafeToString_ReturnsValue_WhenNotNull_AndDefaultProvided()
    {
        // arrange
        string? input = "hello";
        var defaultValue = "default";

        // act
        var actual = input.SafeToString(defaultValue);

        // assert
        actual.ShouldEqual("hello", "SafeToString should return the input value, not the default.");
    }

    #endregion

    #region GetNonEmptyStrings Tests

    [Fact]
    public void GetNonEmptyStrings_ReturnsNonEmptyStrings()
    {
        // arrange
        var input = new string?[] { "one", "two", "three" };

        // act
        var actual = StringExtensionMethods.GetNonEmptyStrings(input);

        // assert
        actual.ShouldHaveCount(3, "Should return all non-empty strings.");
        actual[0].ShouldEqual("one", "First element wrong.");
        actual[1].ShouldEqual("two", "Second element wrong.");
        actual[2].ShouldEqual("three", "Third element wrong.");
    }

    [Fact]
    public void GetNonEmptyStrings_FiltersOutNulls()
    {
        // arrange
        var input = new string?[] { "one", null, "three" };

        // act
        var actual = StringExtensionMethods.GetNonEmptyStrings(input);

        // assert
        actual.ShouldHaveCount(2, "Should filter out null strings.");
        actual[0].ShouldEqual("one", "First element wrong.");
        actual[1].ShouldEqual("three", "Second element wrong.");
    }

    [Fact]
    public void GetNonEmptyStrings_FiltersOutEmptyStrings()
    {
        // arrange
        var input = new string?[] { "one", string.Empty, "three" };

        // act
        var actual = StringExtensionMethods.GetNonEmptyStrings(input);

        // assert
        actual.ShouldHaveCount(2, "Should filter out empty strings.");
        actual[0].ShouldEqual("one", "First element wrong.");
        actual[1].ShouldEqual("three", "Second element wrong.");
    }

    [Fact]
    public void GetNonEmptyStrings_FiltersOutWhitespaceStrings()
    {
        // arrange
        var input = new string?[] { "one", "   ", "three" };

        // act
        var actual = StringExtensionMethods.GetNonEmptyStrings(input);

        // assert
        actual.ShouldHaveCount(2, "Should filter out whitespace strings.");
        actual[0].ShouldEqual("one", "First element wrong.");
        actual[1].ShouldEqual("three", "Second element wrong.");
    }

    [Fact]
    public void GetNonEmptyStrings_ReturnsEmptyArray_WhenAllAreEmpty()
    {
        // arrange
        var input = new string?[] { null, string.Empty, "   " };

        // act
        var actual = StringExtensionMethods.GetNonEmptyStrings(input);

        // assert
        actual.ShouldBeEmpty("Should return empty array when all inputs are null/empty/whitespace.");
    }

    #endregion

    #region ToStringThrowIfNullOrEmpty Tests

    [Fact]
    public void ToStringThrowIfNullOrEmpty_ReturnsValue_WhenNotNull()
    {
        // arrange
        string? input = "hello";

        // act
        var actual = input.ToStringThrowIfNullOrEmpty();

        // assert
        actual.ShouldEqual("hello", "Should return the input value.");
    }

    [Fact]
    public void ToStringThrowIfNullOrEmpty_ThrowsException_WhenNull()
    {
        // arrange
        string? input = null;

        // act & assert
        Assert.Throws<InvalidOperationException>(() => input.ToStringThrowIfNullOrEmpty());
    }

    [Fact]
    public void ToStringThrowIfNullOrEmpty_ThrowsException_WhenEmpty()
    {
        // arrange
        string? input = string.Empty;

        // act & assert
        Assert.Throws<InvalidOperationException>(() => input.ToStringThrowIfNullOrEmpty());
    }

    [Fact]
    public void ToStringThrowIfNullOrEmpty_ThrowsExceptionWithLabel_WhenNullAndLabelProvided()
    {
        // arrange
        string? input = null;
        var label = "MyField";

        // act & assert
        var ex = Assert.Throws<InvalidOperationException>(() => input.ToStringThrowIfNullOrEmpty(label));
        ex.Message.ShouldContain("MyField", "Exception message should contain the label.");
    }

    [Fact]
    public void ToStringThrowIfNullOrEmpty_ThrowsExceptionWithDefaultMessage_WhenNullAndNoLabel()
    {
        // arrange
        string? input = null;

        // act & assert
        var ex = Assert.Throws<InvalidOperationException>(() => input.ToStringThrowIfNullOrEmpty());
        ex.Message.ShouldContain("Input string", "Exception message should contain default text.");
    }

    #endregion

    #region EqualsCaseInsensitive Tests

    [Fact]
    public void EqualsCaseInsensitive_ReturnsTrue_WhenBothNull()
    {
        // arrange
        string? value1 = null;
        string? value2 = null;

        // act
        var actual = value1.EqualsCaseInsensitive(value2);

        // assert
        actual.ShouldBeTrue("Both null values should be considered equal.");
    }

    [Fact]
    public void EqualsCaseInsensitive_ReturnsFalse_WhenFirstIsNull()
    {
        // arrange
        string? value1 = null;
        string? value2 = "hello";

        // act
        var actual = value1.EqualsCaseInsensitive(value2);

        // assert
        actual.ShouldBeFalse("Null and non-null should not be equal.");
    }

    [Fact]
    public void EqualsCaseInsensitive_ReturnsFalse_WhenSecondIsNull()
    {
        // arrange
        string? value1 = "hello";
        string? value2 = null;

        // act
        var actual = value1.EqualsCaseInsensitive(value2);

        // assert
        actual.ShouldBeFalse("Non-null and null should not be equal.");
    }

    [Fact]
    public void EqualsCaseInsensitive_ReturnsTrue_WhenSameCase()
    {
        // arrange
        string? value1 = "hello";
        string? value2 = "hello";

        // act
        var actual = value1.EqualsCaseInsensitive(value2);

        // assert
        actual.ShouldBeTrue("Same strings should be equal.");
    }

    [Fact]
    public void EqualsCaseInsensitive_ReturnsTrue_WhenDifferentCase()
    {
        // arrange
        string? value1 = "HELLO";
        string? value2 = "hello";

        // act
        var actual = value1.EqualsCaseInsensitive(value2);

        // assert
        actual.ShouldBeTrue("Strings with different case should be equal.");
    }

    [Fact]
    public void EqualsCaseInsensitive_ReturnsFalse_WhenDifferentValues()
    {
        // arrange
        string? value1 = "hello";
        string? value2 = "world";

        // act
        var actual = value1.EqualsCaseInsensitive(value2);

        // assert
        actual.ShouldBeFalse("Different strings should not be equal.");
    }

    #endregion

    #region SafeToInt32 Tests

    [Fact]
    public void SafeToInt32_ReturnsValue_WhenValidInteger()
    {
        // arrange
        string? input = "42";

        // act
        var actual = input.SafeToInt32();

        // assert
        actual.ShouldEqual(42, "Should parse valid integer string.");
    }

    [Fact]
    public void SafeToInt32_ReturnsDefault_WhenNull()
    {
        // arrange
        string? input = null;

        // act
        var actual = input.SafeToInt32();

        // assert
        actual.ShouldEqual(0, "Should return default value (0) for null.");
    }

    [Fact]
    public void SafeToInt32_ReturnsDefault_WhenEmpty()
    {
        // arrange
        string? input = string.Empty;

        // act
        var actual = input.SafeToInt32();

        // assert
        actual.ShouldEqual(0, "Should return default value (0) for empty string.");
    }

    [Fact]
    public void SafeToInt32_ReturnsDefault_WhenInvalidNumber()
    {
        // arrange
        string? input = "not a number";

        // act
        var actual = input.SafeToInt32();

        // assert
        actual.ShouldEqual(0, "Should return default value (0) for non-numeric string.");
    }

    [Fact]
    public void SafeToInt32_ReturnsCustomDefault_WhenNull()
    {
        // arrange
        string? input = null;
        var defaultValue = -1;

        // act
        var actual = input.SafeToInt32(defaultValue);

        // assert
        actual.ShouldEqual(-1, "Should return custom default value for null.");
    }

    [Fact]
    public void SafeToInt32_ReturnsCustomDefault_WhenInvalidNumber()
    {
        // arrange
        string? input = "xyz";
        var defaultValue = 99;

        // act
        var actual = input.SafeToInt32(defaultValue);

        // assert
        actual.ShouldEqual(99, "Should return custom default value for non-numeric string.");
    }

    [Fact]
    public void SafeToInt32_ParsesNegativeNumber()
    {
        // arrange
        string? input = "-123";

        // act
        var actual = input.SafeToInt32();

        // assert
        actual.ShouldEqual(-123, "Should parse negative integer string.");
    }

    #endregion

    #region SafeContains Tests

    [Fact]
    public void SafeContains_ReturnsTrue_WhenContainsSubstring()
    {
        // arrange
        string? input = "hello world";
        var searchValue = "world";

        // act
        var actual = input.SafeContains(searchValue);

        // assert
        actual.ShouldBeTrue("Should find substring in string.");
    }

    [Fact]
    public void SafeContains_ReturnsFalse_WhenDoesNotContainSubstring()
    {
        // arrange
        string? input = "hello world";
        var searchValue = "foo";

        // act
        var actual = input.SafeContains(searchValue);

        // assert
        actual.ShouldBeFalse("Should not find missing substring.");
    }

    [Fact]
    public void SafeContains_ReturnsFalse_WhenNull()
    {
        // arrange
        string? input = null;
        var searchValue = "foo";

        // act
        var actual = input.SafeContains(searchValue);

        // assert
        actual.ShouldBeFalse("Should return false for null input.");
    }

    [Fact]
    public void SafeContains_ReturnsFalse_WhenEmpty()
    {
        // arrange
        string? input = string.Empty;
        var searchValue = "foo";

        // act
        var actual = input.SafeContains(searchValue);

        // assert
        actual.ShouldBeFalse("Should return false for empty input.");
    }

    [Fact]
    public void SafeContains_IsCaseSensitive()
    {
        // arrange
        string? input = "hello world";
        var searchValue = "WORLD";

        // act
        var actual = input.SafeContains(searchValue);

        // assert
        actual.ShouldBeFalse("SafeContains should be case-sensitive.");
    }

    #endregion

    #region SafeContainsCaseInsensitive Tests

    [Fact]
    public void SafeContainsCaseInsensitive_ReturnsTrue_WhenContainsSubstring()
    {
        // arrange
        string? input = "hello world";
        var searchValue = "world";

        // act
        var actual = input.SafeContainsCaseInsensitive(searchValue);

        // assert
        actual.ShouldBeTrue("Should find substring in string.");
    }

    [Fact]
    public void SafeContainsCaseInsensitive_ReturnsTrue_WhenContainsSubstringDifferentCase()
    {
        // arrange
        string? input = "hello world";
        var searchValue = "WORLD";

        // act
        var actual = input.SafeContainsCaseInsensitive(searchValue);

        // assert
        actual.ShouldBeTrue("Should find substring regardless of case.");
    }

    [Fact]
    public void SafeContainsCaseInsensitive_ReturnsFalse_WhenDoesNotContainSubstring()
    {
        // arrange
        string? input = "hello world";
        var searchValue = "foo";

        // act
        var actual = input.SafeContainsCaseInsensitive(searchValue);

        // assert
        actual.ShouldBeFalse("Should not find missing substring.");
    }

    [Fact]
    public void SafeContainsCaseInsensitive_ReturnsFalse_WhenNull()
    {
        // arrange
        string? input = null;
        var searchValue = "foo";

        // act
        var actual = input.SafeContainsCaseInsensitive(searchValue);

        // assert
        actual.ShouldBeFalse("Should return false for null input.");
    }

    [Fact]
    public void SafeContainsCaseInsensitive_ReturnsFalse_WhenEmpty()
    {
        // arrange
        string? input = string.Empty;
        var searchValue = "foo";

        // act
        var actual = input.SafeContainsCaseInsensitive(searchValue);

        // assert
        actual.ShouldBeFalse("Should return false for empty input.");
    }

    #endregion

    #region IsNullOrWhitespace Tests

    [Fact]
    public void IsNullOrWhitespace_ReturnsTrue_WhenNull()
    {
        // arrange
        string? input = null;

        // act
        var actual = input.IsNullOrWhitespace();

        // assert
        actual.ShouldBeTrue("Should return true for null.");
    }

    [Fact]
    public void IsNullOrWhitespace_ReturnsTrue_WhenEmpty()
    {
        // arrange
        string? input = string.Empty;

        // act
        var actual = input.IsNullOrWhitespace();

        // assert
        actual.ShouldBeTrue("Should return true for empty string.");
    }

    [Fact]
    public void IsNullOrWhitespace_ReturnsTrue_WhenWhitespace()
    {
        // arrange
        string? input = "   ";

        // act
        var actual = input.IsNullOrWhitespace();

        // assert
        actual.ShouldBeTrue("Should return true for whitespace string.");
    }

    [Fact]
    public void IsNullOrWhitespace_ReturnsFalse_WhenHasValue()
    {
        // arrange
        string? input = "hello";

        // act
        var actual = input.IsNullOrWhitespace();

        // assert
        actual.ShouldBeFalse("Should return false for non-empty string.");
    }

    #endregion

    #region IsNullOrEmpty Tests

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenNull()
    {
        // arrange
        string? input = null;

        // act
        var actual = input.IsNullOrEmpty();

        // assert
        actual.ShouldBeTrue("Should return true for null.");
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsTrue_WhenEmpty()
    {
        // arrange
        string? input = string.Empty;

        // act
        var actual = input.IsNullOrEmpty();

        // assert
        actual.ShouldBeTrue("Should return true for empty string.");
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsFalse_WhenWhitespace()
    {
        // arrange
        string? input = "   ";

        // act
        var actual = input.IsNullOrEmpty();

        // assert
        actual.ShouldBeFalse("Should return false for whitespace string (not empty).");
    }

    [Fact]
    public void IsNullOrEmpty_ReturnsFalse_WhenHasValue()
    {
        // arrange
        string? input = "hello";

        // act
        var actual = input.IsNullOrEmpty();

        // assert
        actual.ShouldBeFalse("Should return false for non-empty string.");
    }

    #endregion
}
