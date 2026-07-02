using System;
using System.Linq;
using System.Text.Json;

using Benday.Common.Json;

using Benday.Common.Testing;

using Xunit;


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
#pragma warning disable CS0618 // exercising obsolete overload intentionally
        var actual = document.RootElement.SafeGetDateTime("non-existent-property", customDefault);
#pragma warning restore CS0618

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

    #region JsonElement GetArray Tests

    [Fact]
    public void GetArray_ReturnsArray_WhenPropertyExists()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArray("users");

        // assert
        AssertThat.AreEqual(true, actual.HasValue, "Should return the array element");
        AssertThat.AreEqual(JsonValueKind.Array, actual!.Value.ValueKind, "Element should be an array");
        AssertThat.AreEqual(3, actual.Value.GetArrayLength(), "Array length");
    }

    [Fact]
    public void GetArray_ReturnsArray_WhenNestedPath()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArray("settings", "options");

        // assert
        AssertThat.AreEqual(true, actual.HasValue, "Should return the nested array element");
        AssertThat.AreEqual(JsonValueKind.Array, actual!.Value.ValueKind, "Element should be an array");
        AssertThat.AreEqual(3, actual.Value.GetArrayLength(), "Array length");
    }

    [Fact]
    public void GetArray_ReturnsNull_WhenPropertyDoesNotExist()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArray("non-existent-property");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "Missing array property should return null");
    }

    [Fact]
    public void GetArray_ReturnsNull_WhenPropertyIsNotAnArray()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArray("top-level-property");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "Non-array property should return null");
    }

    [Fact]
    public void GetArray_ReturnsEmptyArray_WhenArrayIsEmpty()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArray("empty-array");

        // assert
        AssertThat.AreEqual(true, actual.HasValue, "Empty array should still be returned");
        AssertThat.AreEqual(0, actual!.Value.GetArrayLength(), "Empty array length");
    }

    #endregion

    #region JsonElement GetArrayItem Tests

    [Fact]
    public void GetArrayItem_ReturnsMatchingItem()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArrayItem("users", "id", "456");

        // assert
        AssertThat.AreEqual(true, actual.HasValue, "Should find matching item");
        AssertThat.AreEqual("Bob", actual!.Value.SafeGetString("name"), "Matched item's name");
    }

    [Fact]
    public void GetArrayItem_ReturnsNull_WhenNoItemMatches()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArrayItem("users", "id", "999");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "No matching item should return null");
    }

    [Fact]
    public void GetArrayItem_ReturnsNull_WhenArrayPropertyMissing()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetArrayItem("non-existent-array", "id", "123");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "Missing array property should return null");
    }

    #endregion

    #region JsonElement SafeGetArrayItem Tests

    [Fact]
    public void SafeGetArrayItem_ReturnsMatchingItem()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var array = document.RootElement.GetArray("users")!.Value;

        // act
        var actual = array.SafeGetArrayItem("name", "Charlie");

        // assert
        AssertThat.AreEqual(true, actual.HasValue, "Should find matching item");
        AssertThat.AreEqual("789", actual!.Value.SafeGetString("id"), "Matched item's id");
    }

    [Fact]
    public void SafeGetArrayItem_ReturnsNull_WhenNoMatch()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var array = document.RootElement.GetArray("users")!.Value;

        // act
        var actual = array.SafeGetArrayItem("name", "Zelda");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "No match should return null");
    }

    [Fact]
    public void SafeGetArrayItem_ReturnsNull_WhenNotAnArray()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var notArray = document.RootElement.GetProperty("top-level-property");

        // act
        var actual = notArray.SafeGetArrayItem("name", "Alice");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "Non-array should return null");
    }

    #endregion

    #region JsonElement FirstOrDefaultWithPropertyName Tests

    [Fact]
    public void FirstOrDefaultWithPropertyName_ReturnsValueOfFirstMatch()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var array = document.RootElement.GetArray("mixed-items")!.Value;

        // act
        var actual = array.FirstOrDefaultWithPropertyName("code");

        // assert
        AssertThat.AreEqual(true, actual.HasValue, "Should find first item with property");
        AssertThat.AreEqual("X1", actual!.Value.GetString(), "Returned value is the property value of first match");
    }

    [Fact]
    public void FirstOrDefaultWithPropertyName_ReturnsNull_WhenNoItemHasProperty()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var array = document.RootElement.GetArray("mixed-items")!.Value;

        // act
        var actual = array.FirstOrDefaultWithPropertyName("non-existent");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "No item with property should return null");
    }

    [Fact]
    public void FirstOrDefaultWithPropertyName_ReturnsNull_WhenNotAnArray()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var notArray = document.RootElement.GetProperty("object-property");

        // act
        var actual = notArray.FirstOrDefaultWithPropertyName("nested-property");

        // assert
        AssertThat.AreEqual(false, actual.HasValue, "Non-array should return null");
    }

    #endregion

    #region JsonElement GetDictionary Tests

    [Fact]
    public void GetDictionary_FromArrayElement_ReturnsKeyValuePairs()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var array = document.RootElement.GetArray("settings", "options")!.Value;

        // act
        var actual = array.GetDictionary("key", "value");

        // assert
        AssertThat.AreEqual(3, actual.Count, "Dictionary count");
        AssertThat.AreEqual("dark", actual["theme"], "theme value");
        AssertThat.AreEqual("en", actual["language"], "language value");
        AssertThat.AreEqual("UTC", actual["timezone"], "timezone value");
    }

    [Fact]
    public void GetDictionary_FromArrayElement_ReturnsEmpty_WhenNotAnArray()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);
        var notArray = document.RootElement.GetProperty("top-level-property");

        // act
        var actual = notArray.GetDictionary("key", "value");

        // assert
        AssertThat.AreEqual(0, actual.Count, "Non-array yields empty dictionary");
    }

    [Fact]
    public void GetDictionary_FromObject_ReturnsKeyValuePairs()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetProperty("settings").GetDictionary("options", "key", "value");

        // assert
        AssertThat.AreEqual(3, actual.Count, "Dictionary count");
        AssertThat.AreEqual("dark", actual["theme"], "theme value");
    }

    [Fact]
    public void GetDictionary_FromObject_ReturnsEmpty_WhenArrayPropertyMissing()
    {
        // arrange
        var json = GetSampleFileText("json-extension-method-testing.json");
        var document = JsonDocument.Parse(json);

        // act
        var actual = document.RootElement.GetDictionary("non-existent-array", "key", "value");

        // assert
        AssertThat.AreEqual(0, actual.Count, "Missing array yields empty dictionary");
    }

    #endregion
}