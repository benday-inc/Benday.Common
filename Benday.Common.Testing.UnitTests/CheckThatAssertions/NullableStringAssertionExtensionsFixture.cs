using System;
using Benday.Common.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class NullableStringAssertionExtensionsFixture : TestClassBase
{
    public NullableStringAssertionExtensionsFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region IsEqualTo Tests

    [Fact]
    public void IsEqualTo_BothNull_DoesNotThrow()
    {
        string? input = null;
        string? expected = null;

        input.CheckThatNullable().IsEqualTo(expected);
    }

    [Fact]
    public void IsEqualTo_InputNullExpectedNotNull_Throws()
    {
        string? input = null;
        string? expected = "value";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsEqualTo(expected));
        
        Assert.Contains("Expected 'value' but actual value was ''", ex.Message);
    }

    [Fact]
    public void IsEqualTo_InputNotNullExpectedNull_Throws()
    {
        string? input = "value";
        string? expected = null;

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsEqualTo(expected));
        
        Assert.Contains("Expected '' but actual value was 'value'", ex.Message);
    }

    [Fact]
    public void IsEqualTo_WithCustomMessage_UsesCustomMessage()
    {
        string? input = "abc";
        string? expected = "def";
        var customMessage = "Custom equality failure";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsEqualTo(expected, customMessage));
        
        Assert.Equal(customMessage, ex.Message);
    }

    #endregion

    #region IsEqualCaseInsensitiveTo Tests

    [Fact]
    public void IsEqualCaseInsensitiveTo_BothNull_DoesNotThrow()
    {
        string? input = null;
        string? expected = null;

        input.CheckThatNullable().IsEqualCaseInsensitiveTo(expected);
    }

    [Fact]
    public void IsEqualCaseInsensitiveTo_DifferentCase_DoesNotThrow()
    {
        string? input = "Hello World";
        string? expected = "HELLO WORLD";

        input.CheckThatNullable().IsEqualCaseInsensitiveTo(expected);
    }

    [Fact]
    public void IsEqualCaseInsensitiveTo_InputNullExpectedNotNull_Throws()
    {
        string? input = null;
        string? expected = "VALUE";

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsEqualCaseInsensitiveTo(expected));
    }

    [Fact]
    public void IsEqualCaseInsensitiveTo_WithCustomMessage_UsesCustomMessage()
    {
        string? input = "abc";
        string? expected = "def";
        var customMessage = "Case insensitive equality failed";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsEqualCaseInsensitiveTo(expected, customMessage));
        
        Assert.Equal(customMessage, ex.Message);
    }

    #endregion

    #region IsNotEqualTo Tests

    [Fact]
    public void IsNotEqualTo_BothNull_Throws()
    {
        string? input = null;
        string? expected = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotEqualTo(expected));
    }

    [Fact]
    public void IsNotEqualTo_InputNullExpectedNotNull_DoesNotThrow()
    {
        string? input = null;
        string? expected = "value";

        input.CheckThatNullable().IsNotEqualTo(expected);
    }

    [Fact]
    public void IsNotEqualTo_WithCustomMessage_UsesCustomMessage()
    {
        string? input = "same";
        string? expected = "same";
        var customMessage = "Values should be different";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotEqualTo(expected, customMessage));
        
        Assert.Equal(customMessage, ex.Message);
    }

    #endregion

    #region IsNotNullOrEmpty Tests

    [Fact]
    public void IsNotNullOrEmpty_Null_ThrowsWithNullMessage()
    {
        string? input = null;

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotNullOrEmpty());
        
        Assert.Equal("Input is null.", ex.Message);
    }

    [Fact]
    public void IsNotNullOrEmpty_Empty_ThrowsWithEmptyMessage()
    {
        string? input = "";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotNullOrEmpty());
        
        Assert.Equal("String is empty", ex.Message);
    }

    [Fact]
    public void IsNotNullOrEmpty_ValidString_DoesNotThrow()
    {
        string? input = "valid";

        input.CheckThatNullable().IsNotNullOrEmpty();
    }

    [Fact]
    public void IsNotNullOrEmpty_WithCustomMessage_UsesCustomMessage()
    {
        string? input = null;
        var customMessage = "String must have a value";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotNullOrEmpty(customMessage));
        
        Assert.Equal(customMessage, ex.Message);
    }

    #endregion

    #region IsNotNullOrWhitespace Tests

    [Fact]
    public void IsNotNullOrWhitespace_Whitespace_ThrowsWithWhitespaceMessage()
    {
        string? input = "   \t\n  ";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotNullOrWhitespace());
        
        Assert.Equal("String is empty or whitespace", ex.Message);
    }

    [Fact]
    public void IsNotNullOrWhitespace_Null_ThrowsWithNullMessage()
    {
        string? input = null;

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotNullOrWhitespace());
        
        Assert.Equal("Input is null.", ex.Message);
    }

    [Fact]
    public void IsNotNullOrWhitespace_WithCustomMessage_UsesCustomMessage()
    {
        string? input = "   ";
        var customMessage = "Must not be whitespace";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().IsNotNullOrWhitespace(customMessage));
        
        Assert.Equal(customMessage, ex.Message);
    }

    #endregion

    #region Contains Tests

    [Fact]
    public void Contains_NullInput_Throws()
    {
        string? input = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().Contains("test"));
    }

    [Fact]
    public void Contains_NotFound_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().Contains("xyz"));
        
        Assert.Equal("Expected 'Hello World' to contain 'xyz'.", ex.Message);
    }

    [Fact]
    public void Contains_Found_DoesNotThrow()
    {
        string? input = "Hello World";

        input.CheckThatNullable().Contains("World");
    }

    [Fact]
    public void Contains_WithCustomMessage_UsesCustomMessage()
    {
        string? input = "test";
        var customMessage = "Substring not found";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().Contains("xyz", customMessage));
        
        Assert.Equal(customMessage, ex.Message);
    }

    #endregion

    #region ContainsCaseInsensitive Tests

    [Fact]
    public void ContainsCaseInsensitive_NullInput_Throws()
    {
        string? input = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().ContainsCaseInsensitive("test"));
    }

    [Fact]
    public void ContainsCaseInsensitive_DifferentCase_DoesNotThrow()
    {
        string? input = "Hello World";

        input.CheckThatNullable().ContainsCaseInsensitive("WORLD");
    }

    [Fact]
    public void ContainsCaseInsensitive_NotFound_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().ContainsCaseInsensitive("xyz"));
        
        Assert.Equal("Expected 'Hello World' to contain 'xyz' (case-insensitive).", ex.Message);
    }

    #endregion

    #region DoesNotContain Tests

    [Fact]
    public void DoesNotContain_NullInput_DoesNotThrow()
    {
        string? input = null;

        input.CheckThatNullable().DoesNotContain("test");
    }

    [Fact]
    public void DoesNotContain_Found_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().DoesNotContain("World"));
        
        Assert.Equal("Expected 'Hello World' to NOT contain 'World'.", ex.Message);
    }

    [Fact]
    public void DoesNotContain_NotFound_DoesNotThrow()
    {
        string? input = "Hello World";

        input.CheckThatNullable().DoesNotContain("xyz");
    }

    #endregion

    #region DoesNotContainCaseInsensitive Tests

    [Fact]
    public void DoesNotContainCaseInsensitive_NullInput_DoesNotThrow()
    {
        string? input = null;

        input.CheckThatNullable().DoesNotContainCaseInsensitive("test");
    }

    [Fact]
    public void DoesNotContainCaseInsensitive_FoundDifferentCase_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().DoesNotContainCaseInsensitive("WORLD"));
        
        Assert.Equal("Expected 'Hello World' to NOT contain 'WORLD' (case-insensitive).", ex.Message);
    }

    #endregion

    #region StartsWith Tests

    [Fact]
    public void StartsWith_NullInput_Throws()
    {
        string? input = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().StartsWith("test"));
    }

    [Fact]
    public void StartsWith_DoesNotStartWith_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().StartsWith("World"));
        
        Assert.Equal("Expected 'Hello World' to start with 'World'.", ex.Message);
    }

    #endregion

    #region StartsWithCaseInsensitive Tests

    [Fact]
    public void StartsWithCaseInsensitive_NullInput_Throws()
    {
        string? input = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().StartsWithCaseInsensitive("test"));
    }

    [Fact]
    public void StartsWithCaseInsensitive_DifferentCase_DoesNotThrow()
    {
        string? input = "Hello World";

        input.CheckThatNullable().StartsWithCaseInsensitive("HELLO");
    }

    #endregion

    #region DoesNotStartWith Tests

    [Fact]
    public void DoesNotStartWith_NullInput_DoesNotThrow()
    {
        string? input = null;

        input.CheckThatNullable().DoesNotStartWith("test");
    }

    [Fact]
    public void DoesNotStartWith_StartsWithValue_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().DoesNotStartWith("Hello"));
        
        Assert.Equal("Expected 'Hello World' to NOT start with 'Hello'.", ex.Message);
    }

    #endregion

    #region DoesNotStartWithCaseInsensitive Tests

    [Fact]
    public void DoesNotStartWithCaseInsensitive_NullInput_DoesNotThrow()
    {
        string? input = null;

        input.CheckThatNullable().DoesNotStartWithCaseInsensitive("test");
    }

    [Fact]
    public void DoesNotStartWithCaseInsensitive_StartsWithDifferentCase_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().DoesNotStartWithCaseInsensitive("HELLO"));
        
        Assert.Equal("Expected 'Hello World' to NOT start with 'HELLO' (case-insensitive).", ex.Message);
    }

    #endregion

    #region EndsWith Tests

    [Fact]
    public void EndsWith_NullInput_Throws()
    {
        string? input = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().EndsWith("test"));
    }

    [Fact]
    public void EndsWith_DoesNotEndWith_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().EndsWith("Hello"));
        
        Assert.Equal("Expected 'Hello World' to end with 'Hello'.", ex.Message);
    }

    #endregion

    #region EndsWithCaseInsensitive Tests

    [Fact]
    public void EndsWithCaseInsensitive_NullInput_Throws()
    {
        string? input = null;

        Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().EndsWithCaseInsensitive("test"));
    }

    [Fact]
    public void EndsWithCaseInsensitive_DifferentCase_DoesNotThrow()
    {
        string? input = "Hello World";

        input.CheckThatNullable().EndsWithCaseInsensitive("WORLD");
    }

    #endregion

    #region DoesNotEndWith Tests

    [Fact]
    public void DoesNotEndWith_NullInput_DoesNotThrow()
    {
        string? input = null;

        input.CheckThatNullable().DoesNotEndWith("test");
    }

    [Fact]
    public void DoesNotEndWith_EndsWithValue_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().DoesNotEndWith("World"));
        
        Assert.Equal("Expected 'Hello World' to NOT end with 'World'.", ex.Message);
    }

    #endregion

    #region DoesNotEndWithCaseInsensitive Tests

    [Fact]
    public void DoesNotEndWithCaseInsensitive_NullInput_DoesNotThrow()
    {
        string? input = null;

        input.CheckThatNullable().DoesNotEndWithCaseInsensitive("test");
    }

    [Fact]
    public void DoesNotEndWithCaseInsensitive_EndsWithDifferentCase_Throws()
    {
        string? input = "Hello World";

        var ex = Assert.Throws<CheckAssertionFailureException>(() =>
            input.CheckThatNullable().DoesNotEndWithCaseInsensitive("WORLD"));
        
        Assert.Equal("Expected 'Hello World' to NOT end with 'WORLD' (case-insensitive).", ex.Message);
    }

    #endregion
}