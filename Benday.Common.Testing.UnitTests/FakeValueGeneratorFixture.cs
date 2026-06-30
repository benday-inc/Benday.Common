using System;
using System.Text;

namespace Benday.Common.Testing.UnitTests;

public class FakeValueGeneratorFixture : TestClassBase
{
    public FakeValueGeneratorFixture(ITestOutputHelper output) : base(output)
    {
    }

    #region Null guards

    [Fact]
    public void GetFakeValueForInt_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForInt(null!));

    [Fact]
    public void GetFakeValueForDouble_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForDouble(null!));

    [Fact]
    public void GetFakeValueForFloat_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForFloat(null!));

    [Fact]
    public void GetFakeValueForByteArray_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForByteArray(null!));

    [Fact]
    public void GetFakeValueForString_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForString(null!));

    [Fact]
    public void GetFakeValueForUrl_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForUrl(null!));

    [Fact]
    public void GetFakeValueForDateTime_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForDateTime(null!));

    #endregion

    #region Predictable mode is deterministic

    [Fact]
    public void Predictable_Int_IsDeterministic()
    {
        Assert.Equal("SomeField".Length, FakeValueGenerator.GetFakeValueForInt("SomeField"));
        Assert.Equal(FakeValueGenerator.GetFakeValueForInt("SomeField"), FakeValueGenerator.GetFakeValueForInt("SomeField"));
    }

    [Fact]
    public void Predictable_Double_IsDeterministic()
    {
        Assert.Equal("SomeField".Length, FakeValueGenerator.GetFakeValueForDouble("SomeField"));
    }

    [Fact]
    public void Predictable_Float_IsDeterministic()
    {
        Assert.Equal("SomeField".Length, FakeValueGenerator.GetFakeValueForFloat("SomeField"));
    }

    [Fact]
    public void Predictable_String_IsDeterministic()
    {
        Assert.Equal("SomeField_0", FakeValueGenerator.GetFakeValueForString("SomeField"));
    }

    [Fact]
    public void Predictable_Url_IsDeterministic()
    {
        Assert.Equal(
            "http://www.fakestuff.com/images/SomeField_0.jpg",
            FakeValueGenerator.GetFakeValueForUrl("SomeField"));
    }

    [Fact]
    public void Predictable_ByteArray_IsDeterministic()
    {
        var actual = FakeValueGenerator.GetFakeValueForByteArray("SomeField");

        Assert.Equal("SomeField_0", Encoding.UTF8.GetString(actual));
    }

    [Fact]
    public void Predictable_DateTime_IsDeterministicAndInThePast()
    {
        var first = FakeValueGenerator.GetFakeValueForDateTime("SomeField");
        var second = FakeValueGenerator.GetFakeValueForDateTime("SomeField");

        Assert.Equal(first, second);
        Assert.True(first < DateTime.Now, "Predictable date should be offset from the fixed base date, not now.");
    }

    #endregion

    #region Seed value uniquifies predictable values across a collection

    [Fact]
    public void Predictable_SeedValue_ProducesDistinctValues()
    {
        var fieldName = "SomeField";

        Assert.NotEqual(
            FakeValueGenerator.GetFakeValueForInt(fieldName, seedValue: 0),
            FakeValueGenerator.GetFakeValueForInt(fieldName, seedValue: 1));

        Assert.NotEqual(
            FakeValueGenerator.GetFakeValueForString(fieldName, seedValue: 0),
            FakeValueGenerator.GetFakeValueForString(fieldName, seedValue: 1));

        Assert.NotEqual(
            FakeValueGenerator.GetFakeValueForDateTime(fieldName, seedValue: 0),
            FakeValueGenerator.GetFakeValueForDateTime(fieldName, seedValue: 1));
    }

    #endregion

    #region Randomize mode varies

    [Fact]
    public void Randomize_Int_VariesBetweenCalls()
    {
        var first = FakeValueGenerator.GetFakeValueForInt("SomeField", randomize: true);
        var second = FakeValueGenerator.GetFakeValueForInt("SomeField", randomize: true);

        WriteLine($"first: {first}, second: {second}");

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Randomize_ByteArray_ReturnsRandomBytes()
    {
        var first = FakeValueGenerator.GetFakeValueForByteArray("SomeField", randomize: true);
        var second = FakeValueGenerator.GetFakeValueForByteArray("SomeField", randomize: true);

        Assert.NotEqual(first, second);
    }

    #endregion

    #region Additional scalar types

    [Fact]
    public void GetFakeValueForLong_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForLong(null!));

    [Fact]
    public void GetFakeValueForBool_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForBool(null!));

    [Fact]
    public void GetFakeValueForDecimal_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForDecimal(null!));

    [Fact]
    public void GetFakeValueForGuid_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForGuid(null!));

    [Fact]
    public void GetFakeValueForEnum_NullFieldName_Throws() =>
        Assert.Throws<ArgumentNullException>(() => FakeValueGenerator.GetFakeValueForEnum<SampleStatus>(null!));

    [Fact]
    public void GetFakeValueForEnum_NonEnumType_Throws() =>
        Assert.Throws<ArgumentException>(() => FakeValueGenerator.GetFakeValueForEnum(typeof(int), "SomeField"));

    [Fact]
    public void Predictable_Long_IsDeterministic()
    {
        Assert.Equal((long)"SomeField".Length, FakeValueGenerator.GetFakeValueForLong("SomeField"));
    }

    [Theory]
    [InlineData("SomeField", false, 0)]
    [InlineData("SomeField", false, 1)]
    [InlineData("ab", false, 0)]
    [InlineData("SomeField", true, 0)]
    public void Bool_IsAlwaysTrue(string fieldName, bool randomize, int seedValue)
    {
        // false is default(bool), so a fake bool must never be false or it would look unset
        // to AssertThat.AllPropertiesAreNonNullAndNonDefaultValue.
        Assert.True(FakeValueGenerator.GetFakeValueForBool(fieldName, randomize, seedValue));
    }

    [Fact]
    public void Predictable_Decimal_IsDeterministic()
    {
        Assert.Equal("SomeField".Length, FakeValueGenerator.GetFakeValueForDecimal("SomeField"));
    }

    [Fact]
    public void Predictable_Guid_IsDeterministicAndSeedVaries()
    {
        var first = FakeValueGenerator.GetFakeValueForGuid("SomeField");
        var second = FakeValueGenerator.GetFakeValueForGuid("SomeField");

        Assert.Equal(first, second);
        Assert.NotEqual(Guid.Empty, first);
        Assert.NotEqual(
            FakeValueGenerator.GetFakeValueForGuid("SomeField", seedValue: 0),
            FakeValueGenerator.GetFakeValueForGuid("SomeField", seedValue: 1));
    }

    [Fact]
    public void Predictable_Enum_IsDeterministic()
    {
        var first = FakeValueGenerator.GetFakeValueForEnum<SampleStatus>("SomeField");
        var second = FakeValueGenerator.GetFakeValueForEnum<SampleStatus>("SomeField");

        Assert.Equal(first, second);
    }

    [Fact]
    public void GetFakeValueForEnum_GenericAndNonGeneric_Agree()
    {
        var generic = FakeValueGenerator.GetFakeValueForEnum<SampleStatus>("SomeField");
        var nonGeneric = FakeValueGenerator.GetFakeValueForEnum(typeof(SampleStatus), "SomeField");

        Assert.Equal(generic, (SampleStatus)nonGeneric);
    }

    #endregion
}
