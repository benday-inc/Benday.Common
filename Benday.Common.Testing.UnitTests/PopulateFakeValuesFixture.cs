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

    #region Create

    [Fact]
    public void Create_SingleInstance_IsPopulated()
    {
        var instance = FakeValueGenerator.Create<FullySupportedModel>();

        Assert.NotEqual(0, instance.IntValue);
        Assert.Equal("StringValue_0", instance.StringValue);
        Assert.NotEqual(Guid.Empty, instance.GuidValue);
    }

    [Fact]
    public void Create_SingleInstance_SkipPropertyNames_AreLeftUntouched()
    {
        var instance = FakeValueGenerator.Create<FullySupportedModel>(
            skipPropertyNames: ["IntValue"]);

        Assert.Equal(0, instance.IntValue);
        Assert.NotEmpty(instance.StringValue);
    }

    [Fact]
    public void Create_SingleInstance_WithHandler_PopulatesSpecialCaseProperties()
    {
        var instance = FakeValueGenerator.Create<MixedModel>(
            prop =>
            {
                if (prop.PropertyName == nameof(MixedModel.Custom))
                {
                    prop.Instance.Custom = new CustomThing { Value = "custom" };
                    prop.IsHandled = true;
                }
            });

        Assert.NotNull(instance.Custom);
        Assert.Equal("custom", instance.Custom!.Value);
    }

    [Fact]
    public void Create_NegativeCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FakeValueGenerator.Create<FullySupportedModel>(-1));
    }

    [Fact]
    public void Create_ZeroCount_ReturnsEmptyList()
    {
        var result = FakeValueGenerator.Create<FullySupportedModel>(0);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ReturnsRequestedNumberOfPopulatedInstances()
    {
        var result = FakeValueGenerator.Create<FullySupportedModel>(3);

        Assert.Equal(3, result.Count);

        foreach (var instance in result)
        {
            Assert.NotEqual(0, instance.IntValue);
            Assert.NotEmpty(instance.StringValue);
            Assert.NotEqual(Guid.Empty, instance.GuidValue);
        }
    }

    [Fact]
    public void Create_UsesIndexAsSeed_SoInstancesAreDistinct()
    {
        var result = FakeValueGenerator.Create<FullySupportedModel>(3);

        Assert.Equal("StringValue_0", result[0].StringValue);
        Assert.Equal("StringValue_1", result[1].StringValue);
        Assert.Equal("StringValue_2", result[2].StringValue);

        var distinctInts = result.Select(x => x.IntValue).Distinct().Count();
        Assert.Equal(3, distinctInts);
    }

    [Fact]
    public void Create_SkipPropertyNames_AreLeftUntouched()
    {
        var result = FakeValueGenerator.Create<FullySupportedModel>(
            2, skipPropertyNames: ["IntValue"]);

        Assert.All(result, x => Assert.Equal(0, x.IntValue));
    }

    [Fact]
    public void Create_WithHandler_PopulatesSpecialCaseProperties()
    {
        var result = FakeValueGenerator.Create<MixedModel>(
            2,
            prop =>
            {
                if (prop.PropertyName == nameof(MixedModel.Custom))
                {
                    prop.Instance.Custom = new CustomThing { Value = $"custom_{prop.SeedValue}" };
                    prop.IsHandled = true;
                }
            });

        Assert.Equal(2, result.Count);
        Assert.Equal("custom_0", result[0].Custom!.Value);
        Assert.Equal("custom_1", result[1].Custom!.Value);
    }

    #endregion

    #region special-case lambda

    [Fact]
    public void Handler_CanPopulateUnsupportedProperty()
    {
        // MixedModel.Custom is a CustomThing, which has no registered generator. Normally it is
        // skipped as UnsupportedType; the handler populates it and marks it handled.
        var result = FakeValueGenerator.PopulateFakeValues(new MixedModel(),
            prop =>
            {
                if (prop.PropertyName == nameof(MixedModel.Custom))
                {
                    prop.Instance.Custom = new CustomThing { Value = $"custom_{prop.SeedValue}" };
                    prop.IsHandled = true;
                }
            });

        Assert.Contains("Custom", result.PopulatedProperties);
        Assert.DoesNotContain(
            result.SkippedProperties,
            x => x.Name == "Custom" && x.Reason == SkipReason.UnsupportedType);
        Assert.NotNull(result.Instance.Custom);
        Assert.Equal("custom_0", result.Instance.Custom!.Value);
    }

    [Fact]
    public void Handler_CanOverrideABuiltInGeneratedValue()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new FullySupportedModel(),
            prop =>
            {
                if (prop.PropertyName == nameof(FullySupportedModel.StringValue))
                {
                    prop.Instance.StringValue = "handled-by-caller";
                    prop.IsHandled = true;
                }
            });

        Assert.Equal("handled-by-caller", result.Instance.StringValue);
        Assert.Contains("StringValue", result.PopulatedProperties);
        // a property the handler ignored still gets a generated value
        Assert.Equal("IntValue".Length, result.Instance.IntValue);
    }

    [Fact]
    public void Handler_UnhandledPropertiesUseBuiltInGeneration()
    {
        var result = FakeValueGenerator.PopulateFakeValues(new FullySupportedModel(),
            prop => { /* observe only, never handle */ });

        // identical outcome to the no-handler overload
        Assert.Empty(result.SkippedProperties);
        Assert.Equal(13, result.PopulatedProperties.Count);
        Assert.Equal("StringValue_0", result.Instance.StringValue);
    }

    #endregion
}
