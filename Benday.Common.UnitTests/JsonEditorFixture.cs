using System;

using Benday.Common.Json;
using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class JsonEditorFixture : TestClassBase
{
    public JsonEditorFixture(ITestOutputHelper output) : base(output)
    {
    }

    private const string SampleJsonPopulated = """
        {
          "FirstLevel": "FirstLevelValue",
          "ConnectionStrings": {
            "DefaultConnectionString": "default-connection-string-value"
          },
          "Logging": {
            "IncludeScopes": false,
            "LogLevel": {
              "Default": "Debug",
              "System": "Information",
              "Microsoft": "Information"
            }
          },
          "CustomConfig": {
            "Value1": "value1-value",
            "Value2": "value2-value"
          }
        }
        """;

    private const string SampleJsonEmpty = """
        {
        }
        """;

    private const string SampleAuthMeItem = """
        {
          "access_token": "access_token_value",
          "expires_on": "2023-03-17T13:24:57.4807320Z",
          "id_token": "id_token_value",
          "provider_name": "aad",
          "user_claims": [
            {
              "typ": "aud",
              "val": "aud-value"
            },
            {
              "typ": "iat",
              "val": "iat-value"
            },
            {
              "typ": "nbf",
              "val": "nbf-value"
            },
            {
              "typ": "exp",
              "val": "exp-value"
            }
          ],
          "user_id": "user_id_value"
        }
        """;

    private JsonEditor CreateSystemUnderTestWithPopulatedJson()
    {
        return new JsonEditor(SampleJsonPopulated, true);
    }

    private JsonEditor CreateSystemUnderTestWithEmptyJson()
    {
        return new JsonEditor(SampleJsonEmpty, true);
    }

    private JsonEditor CreateSystemUnderTestWithAuthMeJson()
    {
        return new JsonEditor(SampleAuthMeItem, true);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_ThrowsException_WhenJsonIsNull()
    {
        // arrange
        string? json = null;

        // act & assert
        Assert.Throws<ArgumentException>(() => new JsonEditor(json!, true));
    }

    [Fact]
    public void Constructor_ThrowsException_WhenJsonIsEmpty()
    {
        // arrange
        var json = string.Empty;

        // act & assert
        Assert.Throws<ArgumentException>(() => new JsonEditor(json, true));
    }

    [Fact]
    public void Constructor_ThrowsException_WhenLoadFromStringIsFalse()
    {
        // arrange
        var json = SampleJsonPopulated;

        // act & assert
        Assert.Throws<InvalidOperationException>(() => new JsonEditor(json, false));
    }

    #endregion

    #region GetValue Tests

    [Fact]
    public void GetValue_OneLevel()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var expected = "FirstLevelValue";

        // act
        var actual = systemUnderTest.GetValue("FirstLevel");

        // assert
        actual.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void GetValue_TwoLevels()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var expected = "default-connection-string-value";

        // act
        var actual = systemUnderTest.GetValue("ConnectionStrings", "DefaultConnectionString");

        // assert
        actual.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void GetValue_ThreeLevels()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var expected = "Debug";

        // act
        var actual = systemUnderTest.GetValue("Logging", "LogLevel", "Default");

        // assert
        actual.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void GetValue_ReturnsNull_WhenNodeDoesNotExist()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.GetValue("NonExistent");

        // assert
        actual.ShouldBeNull("Should return null for non-existent node.");
    }

    [Fact]
    public void GetValue_ThrowsException_WhenNodesIsEmpty()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act & assert
        Assert.Throws<ArgumentException>(() => systemUnderTest.GetValue());
    }

    #endregion

    #region GetValueAsBoolean Tests

    [Fact]
    public void GetValueAsBoolean_ReturnsTrue()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        systemUnderTest.SetValue(true, "BooleanValue");

        // act
        var actual = systemUnderTest.GetValueAsBoolean("BooleanValue");

        // assert
        Assert.True(actual.HasValue, "Return value should not be null.");
        actual.Value.ShouldBeTrue("Value should be true.");
    }

    [Fact]
    public void GetValueAsBoolean_ReturnsFalse()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        systemUnderTest.SetValue(false, "BooleanValue");

        // act
        var actual = systemUnderTest.GetValueAsBoolean("BooleanValue");

        // assert
        Assert.True(actual.HasValue, "Return value should not be null.");
        actual.Value.ShouldBeFalse("Value should be false.");
    }

    [Fact]
    public void GetValueAsBoolean_ReturnsNull_WhenNodeDoesNotExist()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.GetValueAsBoolean("NonExistent");

        // assert
        Assert.False(actual.HasValue, "Should return null for non-existent node.");
    }

    [Fact]
    public void GetValueAsBoolean_ReturnsNull_WhenValueIsNotBoolean()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.GetValueAsBoolean("FirstLevel");

        // assert
        Assert.False(actual.HasValue, "Should return null when value is not a boolean.");
    }

    #endregion

    #region GetValueAsInt32 Tests

    [Fact]
    public void GetValueAsInt32_ReturnsValue()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        var expected = 1234;
        systemUnderTest.SetValue(expected, "Int32Value");

        // act
        var actual = systemUnderTest.GetValueAsInt32("Int32Value");

        // assert
        Assert.True(actual.HasValue, "Return value should not be null.");
        actual.Value.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void GetValueAsInt32_ReturnsNull_WhenNodeDoesNotExist()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.GetValueAsInt32("NonExistent");

        // assert
        Assert.False(actual.HasValue, "Should return null for non-existent node.");
    }

    [Fact]
    public void GetValueAsInt32_ReturnsNull_WhenValueIsNotInt32()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.GetValueAsInt32("FirstLevel");

        // assert
        Assert.False(actual.HasValue, "Should return null when value is not an integer.");
    }

    #endregion

    #region SetValue String Tests

    [Fact]
    public void SetValue_NewValue_OneLevel()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        var expected = "new value";

        // act
        systemUnderTest.SetValue(expected, "FirstLevel");

        // assert
        var actual = systemUnderTest.GetValue("FirstLevel");
        actual.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void SetValue_NewValue_TwoLevels()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        var expected = "new value";

        // act
        systemUnderTest.SetValue(expected, "FirstLevel", "Sub1");
        systemUnderTest.SetValue(expected, "FirstLevel", "Sub2");

        // assert
        var actual1 = systemUnderTest.GetValue("FirstLevel", "Sub1");
        var actual2 = systemUnderTest.GetValue("FirstLevel", "Sub2");
        actual1.ShouldEqual(expected, "Wrong value for Sub1.");
        actual2.ShouldEqual(expected, "Wrong value for Sub2.");
    }

    [Fact]
    public void SetValue_ExistingValue_OneLevel()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var expected = "new-FirstLevelValue";

        // act
        systemUnderTest.SetValue(expected, "FirstLevel");

        // assert
        var actual = systemUnderTest.GetValue("FirstLevel");
        actual.ShouldEqual(expected, "Wrong value.");

        var json = systemUnderTest.ToJson(true);
        json.ShouldContain(expected, "Json did not contain expected string.");
    }

    [Fact]
    public void SetValue_ExistingValue_TwoLevels()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var expected = "new-default-connection-string-value";

        // act
        systemUnderTest.SetValue(expected, "ConnectionStrings", "DefaultConnectionString");

        // assert
        var actual = systemUnderTest.GetValue("ConnectionStrings", "DefaultConnectionString");
        actual.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void SetValue_ExistingValue_ThreeLevels()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var expected = "new-Debug";

        // act
        systemUnderTest.SetValue(expected, "Logging", "LogLevel", "Default");

        // assert
        var actual = systemUnderTest.GetValue("Logging", "LogLevel", "Default");
        actual.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void SetValue_DoesNotClobberUnrelatedValues()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();
        var original = systemUnderTest.GetValue("ConnectionStrings", "DefaultConnectionString");

        // act
        systemUnderTest.SetValue("val1", "ConnectionStrings", "conn1");
        systemUnderTest.SetValue("val2", "ConnectionStrings", "conn2");

        // assert
        var actual1 = systemUnderTest.GetValue("ConnectionStrings", "conn1");
        var actual2 = systemUnderTest.GetValue("ConnectionStrings", "conn2");
        var reloaded = systemUnderTest.GetValue("ConnectionStrings", "DefaultConnectionString");

        actual1.ShouldEqual("val1", "Wrong value for conn1.");
        actual2.ShouldEqual("val2", "Wrong value for conn2.");
        reloaded.ShouldEqual(original, "Original value got messed with.");
    }

    [Fact]
    public void SetValue_NewValues_ConnectionStrings()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();

        // act
        systemUnderTest.SetValue("val1", "ConnectionStrings", "conn1");
        systemUnderTest.SetValue("val2", "ConnectionStrings", "conn2");

        // assert
        var actual1 = systemUnderTest.GetValue("ConnectionStrings", "conn1");
        var actual2 = systemUnderTest.GetValue("ConnectionStrings", "conn2");
        actual1.ShouldEqual("val1", "Wrong value for conn1.");
        actual2.ShouldEqual("val2", "Wrong value for conn2.");
    }

    [Fact]
    public void SetValue_ThrowsException_WhenValueIsNullOrEmpty()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();

        // act & assert
        Assert.Throws<ArgumentException>(() => systemUnderTest.SetValue(string.Empty, "FirstLevel"));
    }

    #endregion

    #region SetValue Boolean Tests

    [Fact]
    public void SetValue_Boolean_True_OneLevel()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        var expected = true;

        // act
        systemUnderTest.SetValue(expected, "BooleanValue");

        // assert
        var actual = systemUnderTest.GetValueAsBoolean("BooleanValue");
        Assert.True(actual.HasValue, "Return value should not be null.");
        actual.Value.ShouldEqual(expected, "Wrong value.");
    }

    [Fact]
    public void SetValue_Boolean_False_OneLevel()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        var expected = false;

        // act
        systemUnderTest.SetValue(expected, "BooleanValue");

        // assert
        var actual = systemUnderTest.GetValueAsBoolean("BooleanValue");
        Assert.True(actual.HasValue, "Return value should not be null.");
        actual.Value.ShouldEqual(expected, "Wrong value.");
    }

    #endregion

    #region SetValue Int32 Tests

    [Fact]
    public void SetValue_Int32_OneLevel()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithEmptyJson();
        var expected = 1234;

        // act
        systemUnderTest.SetValue(expected, "Int32Value");

        // assert
        var actual = systemUnderTest.GetValueAsInt32("Int32Value");
        Assert.True(actual.HasValue, "Return value should not be null.");
        actual.Value.ShouldEqual(expected, "Wrong value.");
    }

    #endregion

    #region GetSiblingValue Tests

    [Fact]
    public void GetSiblingValue_ReturnsValue()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithAuthMeJson();
        var expected = "iat-value";

        var args = new SiblingValueArguments
        {
            SiblingSearchKey = "typ",
            SiblingSearchValue = "iat",
            DesiredNodeKey = "val",
            PathArguments = new[] { "user_claims" }
        };

        // act
        var actual = systemUnderTest.GetSiblingValue(args);

        // assert
        actual.ShouldEqual(expected, "Value was wrong.");
    }

    [Fact]
    public void GetSiblingValue_ReturnsNull_WhenNotFound()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithAuthMeJson();

        var args = new SiblingValueArguments
        {
            SiblingSearchKey = "typ",
            SiblingSearchValue = "nonexistent",
            DesiredNodeKey = "val",
            PathArguments = new[] { "user_claims" }
        };

        // act
        var actual = systemUnderTest.GetSiblingValue(args);

        // assert
        actual.ShouldBeNull("Should return null when sibling not found.");
    }

    #endregion

    #region SetSiblingValue Tests

    [Fact]
    public void SetSiblingValue_UpdateExisting()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithAuthMeJson();
        var expected = "iat-value-new";

        var args = new SiblingValueArguments
        {
            SiblingSearchKey = "typ",
            SiblingSearchValue = "iat",
            DesiredNodeKey = "val",
            PathArguments = new[] { "user_claims" },
            DesiredNodeValue = expected
        };

        // act
        systemUnderTest.SetSiblingValue(args);

        // assert
        var actual = systemUnderTest.GetSiblingValue(args);
        actual.ShouldEqual(expected, "Value was wrong.");
    }

    #endregion

    #region ToJson Tests

    [Fact]
    public void ToJson_ReturnsJsonString()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.ToJson();

        // assert
        actual.ShouldNotBeNullOrEmpty("ToJson should return a non-empty string.");
        actual.ShouldContain("FirstLevelValue", "Should contain expected value.");
    }

    [Fact]
    public void ToJson_Indented_ReturnsFormattedJson()
    {
        // arrange
        var systemUnderTest = CreateSystemUnderTestWithPopulatedJson();

        // act
        var actual = systemUnderTest.ToJson(indented: true);

        // assert
        actual.ShouldNotBeNullOrEmpty("ToJson should return a non-empty string.");
        actual.ShouldContain("\n", "Indented JSON should contain newlines.");
    }

    #endregion
}
