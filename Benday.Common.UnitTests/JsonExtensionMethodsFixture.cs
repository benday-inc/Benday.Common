using System;
using System.Linq;
using System.Text.Json;

using Benday.Common.Json;

using Benday.Common.Testing;

using Xunit;

using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class JsonExtensionMethodsFixture : TestClassBase
{
    public JsonExtensionMethodsFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Theory]
    [InlineData("Top Level Value", "top-level-property", null, null)]
    [InlineData("Nested Value", "object-property", "nested-property", null)]
    [InlineData("42", "object-property", "another-object-property", "deeply-nested-property")]
    [InlineData("", "non-existent-property", null, null)]
    [InlineData("", "top-level-property", "non-existent-property", null)]
    [InlineData("", "object-property", "non-existent-property", null)]
    [InlineData("", "object-property", "another-object-property", "non-existent-property")]
    public void TestMethodWithNullArgs(string expected, string? value1, string? value2, string? value3)
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = System.Text.Json.JsonDocument.Parse(json);

        var nonNullValues = 
            GetNonEmptyStrings(
                value1, value2, value3);

        // act

        var actual = document.RootElement.
            SafeGetString(nonNullValues);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true, "top-level-property", null, null)]
    [InlineData(true, "object-property", "nested-property", null)]
    [InlineData(true, "object-property", "another-object-property", "deeply-nested-property")]
    [InlineData(false, "non-existent-property", null, null)]
    [InlineData(false, "top-level-property", "non-existent-property", null)]
    [InlineData(false, "object-property", "non-existent-property", null)]
    [InlineData(false, "object-property", "another-object-property", "non-existent-property")]
    public void GetElement(bool expected, string? value1, string? value2, string? value3)
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = System.Text.Json.JsonDocument.Parse(json);
        
        var nonNullValues = 
            GetNonEmptyStrings(
                value1, value2, value3);

        // act

        var actual = document.RootElement.
            GetElement(nonNullValues);

        // assert
        AssertThat.AreEqual(expected, actual.Found, "Found property?");
    }

    private string[] GetNonEmptyStrings(
        params string?[] input)
    {
        var returnValue = input.Where(x =>
                string.IsNullOrWhiteSpace(x) == false).
            OfType<string>().
            ToArray();

        return returnValue;
    }

    #region SafeGetInt32 Tests

    [Fact]
    public void SafeGetInt32_ReturnsValue_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetInt32("int-property");

        // assert
        AssertThat.AreEqual(123, actual, "Int32 value");
    }

    [Fact]
    public void SafeGetInt32_ReturnsDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetInt32("non-existent-property");

        // assert
        AssertThat.AreEqual(0, actual, "Default value for non-existent property");
    }

    [Fact]
    public void SafeGetInt32_ReturnsDefault_WhenPropertyIsNull()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetInt32("null-property");

        // assert
        AssertThat.AreEqual(0, actual, "Default value for null property");
    }

    [Fact]
    public void SafeGetInt32_ReturnsNestedValue()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetInt32("nested-numbers", "int-value");

        // assert
        AssertThat.AreEqual(456, actual, "Nested int value");
    }

    #endregion

    #region SafeGetDouble Tests

    [Fact]
    public void SafeGetDouble_ReturnsValue_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetDouble("double-property");

        // assert
        AssertThat.AreEqual(3.14159, actual, "Double value");
    }

    [Fact]
    public void SafeGetDouble_ReturnsDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetDouble("non-existent-property");

        // assert
        AssertThat.AreEqual(0.0, actual, "Default value for non-existent property");
    }

    [Fact]
    public void SafeGetDouble_ReturnsDefault_WhenPropertyIsNull()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetDouble("null-property");

        // assert
        AssertThat.AreEqual(0.0, actual, "Default value for null property");
    }

    [Fact]
    public void SafeGetDouble_ReturnsNestedValue()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetDouble("nested-numbers", "double-value");

        // assert
        AssertThat.AreEqual(2.71828, actual, "Nested double value");
    }

    #endregion

    #region SafeGetLong Tests

    [Fact]
    public void SafeGetLong_ReturnsValue_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetLong("long-property");

        // assert
        AssertThat.AreEqual(9223372036854775807L, actual, "Long value");
    }

    [Fact]
    public void SafeGetLong_ReturnsDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetLong("non-existent-property");

        // assert
        AssertThat.AreEqual(0L, actual, "Default value for non-existent property");
    }

    [Fact]
    public void SafeGetLong_ReturnsDefault_WhenPropertyIsNull()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetLong("null-property");

        // assert
        AssertThat.AreEqual(0L, actual, "Default value for null property");
    }

    [Fact]
    public void SafeGetLong_ReturnsNestedValue_TwoLevels()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetLong("nested-numbers", "long-value");

        // assert
        AssertThat.AreEqual(1234567890123L, actual, "Nested long value (2 levels)");
    }

    [Fact]
    public void SafeGetLong_ReturnsNestedValue_ThreeLevels()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetLong("nested-numbers", "deeper", "long-value");

        // assert
        AssertThat.AreEqual(9876543210L, actual, "Nested long value (3 levels)");
    }

    [Fact]
    public void SafeGetLong_ReturnsCustomDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var customDefault = 999L;

        // act
        var actual = document.RootElement.SafeGetLong("non-existent-property", customDefault);

        // assert
        AssertThat.AreEqual(customDefault, actual, "Custom default value");
    }

    #endregion

    #region SafeGetBool Tests

    [Fact]
    public void SafeGetBool_ReturnsTrue_WhenPropertyIsTrue()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetBool("bool-true-property");

        // assert
        AssertThat.AreEqual(true, actual, "Bool true value");
    }

    [Fact]
    public void SafeGetBool_ReturnsFalse_WhenPropertyIsFalse()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetBool("bool-false-property");

        // assert
        AssertThat.AreEqual(false, actual, "Bool false value");
    }

    [Fact]
    public void SafeGetBool_ReturnsDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetBool("non-existent-property");

        // assert
        AssertThat.AreEqual(false, actual, "Default value for non-existent property");
    }

    [Fact]
    public void SafeGetBool_ReturnsCustomDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetBool("non-existent-property", true);

        // assert
        AssertThat.AreEqual(true, actual, "Custom default value");
    }

    [Fact]
    public void SafeGetBool_ReturnsNestedValue()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetBool("nested-numbers", "bool-value");

        // assert
        AssertThat.AreEqual(true, actual, "Nested bool value");
    }

    #endregion

    #region SafeGetDateTime Tests

    [Fact]
    public void SafeGetDateTime_ReturnsValue_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var expected = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);

        // act
        var actual = document.RootElement.SafeGetDateTime("date-property");

        // assert
        AssertThat.AreEqual(expected, actual, "DateTime value");
    }

    [Fact]
    public void SafeGetDateTime_ReturnsDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetDateTime("non-existent-property");

        // assert
        AssertThat.AreEqual(default(DateTime), actual, "Default value for non-existent property");
    }

    [Fact]
    public void SafeGetDateTime_ReturnsDefault_WhenPropertyIsNull()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetDateTime("null-property");

        // assert
        AssertThat.AreEqual(default(DateTime), actual, "Default value for null property");
    }

    [Fact]
    public void SafeGetDateTime_ReturnsCustomDefault_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var customDefault = new DateTime(2000, 1, 1);

        // act
        var actual = document.RootElement.SafeGetDateTime("non-existent-property", customDefault);

        // assert
        AssertThat.AreEqual(customDefault, actual, "Custom default value");
    }

    [Fact]
    public void SafeGetDateTime_ReturnsNestedValue()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var expected = new DateTime(2023, 12, 25, 0, 0, 0, DateTimeKind.Utc);

        // act
        var actual = document.RootElement.SafeGetDateTime("nested-numbers", "date-value");

        // assert
        AssertThat.AreEqual(expected, actual, "Nested DateTime value");
    }

    #endregion

    #region GetElementOrThrow Tests

    [Fact]
    public void GetElementOrThrow_ReturnsElement_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetElementOrThrow("top-level-property");

        // assert
        AssertThat.AreEqual(JsonValueKind.String, actual.ValueKind, "Element value kind");
        AssertThat.AreEqual("Top Level Value", actual.GetString(), "Element value");
    }

    [Fact]
    public void GetElementOrThrow_ReturnsNestedElement()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetElementOrThrow("object-property", "nested-property");

        // assert
        AssertThat.AreEqual(JsonValueKind.String, actual.ValueKind, "Element value kind");
        AssertThat.AreEqual("Nested Value", actual.GetString(), "Element value");
    }

    [Fact]
    public void GetElementOrThrow_ThrowsException_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act & assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            document.RootElement.GetElementOrThrow("non-existent-property"));

        Assert.Contains("non-existent-property", exception.Message);
    }

    [Fact]
    public void GetElementOrThrow_ThrowsException_WhenNestedPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act & assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            document.RootElement.GetElementOrThrow("object-property", "non-existent-property"));

        Assert.Contains("object-property.non-existent-property", exception.Message);
    }

    #endregion

    #region HasProperty Tests

    [Fact]
    public void HasProperty_ReturnsTrue_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.HasProperty("top-level-property");

        // assert
        AssertThat.AreEqual(true, actual, "Property exists");
    }

    [Fact]
    public void HasProperty_ReturnsFalse_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.HasProperty("non-existent-property");

        // assert
        AssertThat.AreEqual(false, actual, "Property does not exist");
    }

    [Fact]
    public void HasProperty_ReturnsTrue_WhenPropertyIsNull()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.HasProperty("null-property");

        // assert
        AssertThat.AreEqual(true, actual, "Null property still exists");
    }

    #endregion

    #region SafeGetString Additional Tests

    [Fact]
    public void SafeGetString_ReturnsNumberAsString()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetString("int-property");

        // assert
        AssertThat.AreEqual("123", actual, "Number converted to string");
    }

    [Fact]
    public void SafeGetString_ReturnsTrueAsString()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetString("bool-true-property");

        // assert
        AssertThat.AreEqual("True", actual, "True converted to string");
    }

    [Fact]
    public void SafeGetString_ReturnsFalseAsString()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetString("bool-false-property");

        // assert
        AssertThat.AreEqual("False", actual, "False converted to string");
    }

    [Fact]
    public void SafeGetString_ReturnsEmptyString_WhenPropertyIsNull()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.SafeGetString("null-property");

        // assert
        AssertThat.AreEqual("", actual, "Null returns empty string");
    }

    #endregion
}