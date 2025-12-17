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


}