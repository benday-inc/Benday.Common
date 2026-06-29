using System;
using System.Linq;

namespace Benday.Common.Testing.UnitTests;

public class PopulateFakeValuesFixture : TestClassBase
{
    public PopulateFakeValuesFixture(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void PopulateFakeValues_NullInstance_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            FakeValueGenerator.PopulateFakeValues<FullySupportedModel>(null!));
    }

    [Fact]
    public void PopulateFakeValues_FullySupportedModel_PopulatesEveryProperty()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new FullySupportedModel());

        Assert.Empty(result.SkippedProperties);
        Assert.Equal(13, result.PopulatedProperties.Count);

        // these asserts should not throw
        result.AssertNoUnsupportedProperties();
        result.AssertAllPropertiesAccountedFor();

        // spot-check that values are actually populated and predictable
        var model = result.Instance;
        Assert.Equal("StringValue_0", model.StringValue);
        Assert.Equal("IntValue".Length, model.IntValue);
        Assert.NotEqual(Guid.Empty, model.GuidValue);
        Assert.NotEqual(default, model.DateTimeValue);
        Assert.NotNull(model.NullableInt);
        Assert.NotNull(model.NullableStatus);
        Assert.NotEmpty(model.ByteArrayValue);
    }

    [Fact]
    public void PopulateFakeValues_SkipPropertyNames_AreCaseInsensitiveAndLeftUntouched()
    {
        // lower-case name on purpose to prove case-insensitivity
        var result = FakeValueGenerator.PopulateFakeValues(
            new FullySupportedModel(), skipPropertyNames: ["intvalue"]);

        Assert.Equal(0, result.Instance.IntValue);
        Assert.DoesNotContain("IntValue", result.PopulatedProperties);

        var skipped = Assert.Single(result.SkippedProperties);
        Assert.Equal("IntValue", skipped.Name);
        Assert.Equal(SkipReason.ExplicitlySkipped, skipped.Reason);
    }

    [Fact]
    public void PopulateFakeValues_ReadOnlyProperty_RecordedAsNoSetter()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new MixedModel());

        var readOnly = result.SkippedProperties.Single(x => x.Name == "ReadOnlyValue");
        Assert.Equal(SkipReason.NoSetter, readOnly.Reason);
    }

    [Fact]
    public void PopulateFakeValues_UnsupportedType_RecordedAsUnsupported()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new MixedModel());

        var custom = result.SkippedProperties.Single(x => x.Name == "Custom");
        Assert.Equal(SkipReason.UnsupportedType, custom.Reason);
    }

    [Fact]
    public void AssertNoUnsupportedProperties_WithUnsupportedType_Throws()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new MixedModel());

        var ex = Assert.Throws<AssertionException>(() => result.AssertNoUnsupportedProperties());

        WriteLine(ex.Message);
        Assert.Contains("Custom", ex.Message);
    }

    [Fact]
    public void AssertAllPropertiesAccountedFor_WithReadOnlyProperty_Throws()
    {
        // skip the unsupported type so only the read-only property remains unaccounted for
        var result = FakeValueGenerator.PopulateFakeValues(
            new MixedModel(), skipPropertyNames: ["Custom"]);

        // unsupported assert passes because Custom is explicitly skipped...
        result.AssertNoUnsupportedProperties();

        // ...but the read-only property is still a surprise
        var ex = Assert.Throws<AssertionException>(() => result.AssertAllPropertiesAccountedFor());
        Assert.Contains("ReadOnlyValue", ex.Message);
    }

    [Fact]
    public void AssertAllPropertiesAccountedFor_WhenEverythingAccountedFor_PassesAndChains()
    {
        var result = FakeValueGenerator.PopulateFakeValues(
            new MixedModel(), skipPropertyNames: ["Custom", "ReadOnlyValue"]);

        var returned = result.AssertAllPropertiesAccountedFor();

        Assert.Same(result, returned);
    }

    [Fact]
    public void PopulateFakeValues_SeedValue_ProducesDistinctInstances()
    {
        var first = FakeValueGenerator.PopulateFakeValues(new FullySupportedModel(), seedValue: 0).Instance;
        var second = FakeValueGenerator.PopulateFakeValues(new FullySupportedModel(), seedValue: 1).Instance;

        Assert.NotEqual(first.StringValue, second.StringValue);
        Assert.NotEqual(first.IntValue, second.IntValue);
    }

    [Fact]
    public void PopulateFakeValues_Randomize_PopulatesEveryProperty()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new FullySupportedModel(), randomize: true);

        Assert.Empty(result.SkippedProperties);
        Assert.Equal(13, result.PopulatedProperties.Count);
    }

    [Fact]
    public void RegisterGenerator_CustomType_IsPopulated()
    {
        FakeValueGenerator.RegisterGenerator(
            typeof(RegisterOnlyThing),
            (field, randomize, seed) => new RegisterOnlyThing { Value = $"{field}_{seed}" });

        var result = FakeValueGenerator.PopulateFakeValues(new RegisterModel());

        Assert.Contains("Thing", result.PopulatedProperties);
        Assert.Empty(result.SkippedProperties);
        Assert.NotNull(result.Instance.Thing);
        Assert.Equal("Thing_0", result.Instance.Thing!.Value);
    }
}
