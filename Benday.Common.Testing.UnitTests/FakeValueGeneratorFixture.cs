using System;
using System.Text;

namespace Benday.Common.Testing.UnitTests;

public class FakeValueGeneratorFixture : TestClassBase
{
    public FakeValueGeneratorFixture(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetFakeValueForInt_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForInt(null!));
    }

    [Fact]
    public void GetFakeValueForInt_ReturnsValues()
    {
        var actual = FakeValueGenerator.GetFakeValueForInt("SomeField");

        WriteLine($"Value: {actual}");

        Assert.NotEqual(0, actual);
    }

    [Fact]
    public void GetFakeValueForDouble_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForDouble(null!));
    }

    [Fact]
    public void GetFakeValueForFloat_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForFloat(null!));
    }

    [Fact]
    public void GetFakeValueForByteArray_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForByteArray(null!));
    }

    [Fact]
    public void GetFakeValueForByteArray_EncodesFieldName()
    {
        var fieldName = "SomeField";

        var actual = FakeValueGenerator.GetFakeValueForByteArray(fieldName);

        Assert.Equal(fieldName, Encoding.UTF8.GetString(actual));
    }

    [Fact]
    public void GetFakeValueForString_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForString(null!));
    }

    [Fact]
    public void GetFakeValueForString_IncludesFieldName()
    {
        var fieldName = "SomeField";

        var actual = FakeValueGenerator.GetFakeValueForString(fieldName);

        WriteLine($"Value: {actual}");

        Assert.StartsWith($"{fieldName}_", actual);
    }

    [Fact]
    public void GetFakeValueForUrl_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForUrl(null!));
    }

    [Fact]
    public void GetFakeValueForUrl_IncludesFieldName()
    {
        var fieldName = "SomeField";

        var actual = FakeValueGenerator.GetFakeValueForUrl(fieldName);

        WriteLine($"Value: {actual}");

        Assert.Contains(fieldName, actual);
    }

    [Fact]
    public void GetFakeValueForDateTime_NullFieldName_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForDateTime(null!));
    }

    [Fact]
    public void GetFakeValueForDateTime_OffsetsByFieldNameLength()
    {
        var fieldName = "SomeField";

        var before = DateTime.Now;

        var actual = FakeValueGenerator.GetFakeValueForDateTime(fieldName);

        WriteLine($"Value: {actual}");

        // value is offset into the future by the length of the field name (in minutes)
        Assert.True(actual > before, "Expected the generated date to be in the future.");
    }
}