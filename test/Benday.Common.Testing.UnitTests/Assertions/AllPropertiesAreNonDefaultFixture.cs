using System;

namespace Benday.Common.Testing.UnitTests.Assertions;

public class AllPropertiesAreNonDefaultFixture : TestClassBase
{
    public AllPropertiesAreNonDefaultFixture(ITestOutputHelper output) : base(output)
    {
    }

    private static NonDefaultModel CreateFullyPopulated()
    {
        return new NonDefaultModel
        {
            IntValue = 42,
            StringValue = "hello",
            DecimalValue = 12.5m,
            Status = SampleStatus.Active,
            GuidValue = Guid.NewGuid(),
            DateTimeValue = new DateTime(1999, 1, 1),
            NullableInt = 7,
        };
    }

    [Fact]
    public void FullyPopulated_DoesNotThrow()
    {
        var instance = CreateFullyPopulated();

        AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance);
    }

    [Fact]
    public void NullInstance_Throws()
    {
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AllPropertiesAreNonNullAndNonDefaultValue<NonDefaultModel>(null!));

        WriteLine(ex.Message);
        Assert.Contains("non-null instance", ex.Message);
    }

    [Fact]
    public void DefaultIntProperty_IsReported()
    {
        var instance = CreateFullyPopulated();
        instance.IntValue = 0;

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance));

        WriteLine(ex.Message);
        Assert.Contains("1 property was null or set to a default value", ex.Message);
        Assert.Contains("IntValue", ex.Message);
    }

    [Fact]
    public void NullStringProperty_IsReported()
    {
        var instance = CreateFullyPopulated();
        instance.StringValue = null!;

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance));

        WriteLine(ex.Message);
        Assert.Contains("StringValue", ex.Message);
    }

    [Fact]
    public void MultipleDefaults_AreAllReported()
    {
        var instance = CreateFullyPopulated();
        instance.IntValue = 0;
        instance.GuidValue = Guid.Empty;
        instance.NullableInt = null;

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance));

        WriteLine(ex.Message);
        Assert.Contains("3 properties were null or set to a default value", ex.Message);
        Assert.Contains("IntValue", ex.Message);
        Assert.Contains("GuidValue", ex.Message);
        Assert.Contains("NullableInt", ex.Message);
    }

    [Fact]
    public void EmptyString_IsNotTreatedAsDefault()
    {
        var instance = CreateFullyPopulated();
        instance.StringValue = string.Empty;

        // empty string is non-null and therefore non-default(string)
        AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance);
    }

    [Fact]
    public void SkippedProperty_IsNotChecked()
    {
        var instance = CreateFullyPopulated();
        instance.IntValue = 0;

        AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(
            instance, skipPropertyNames: new[] { "IntValue" });
    }

    [Fact]
    public void IntegratesWithPopulateFakeValues()
    {
        var instance = new NonDefaultModel();

        FakeValueGenerator.PopulateFakeValues(instance);

        // Everything the generator touched should now be non-default. Status is excluded because the
        // generator picks an enum member by index ((fieldName.Length + seed) % memberCount), which for
        // "Status" lands on the zero member -- a legitimately "default" enum value.
        AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(
            instance, skipPropertyNames: new[] { nameof(NonDefaultModel.Status) });
    }

    #region special-case lambda

    [Fact]
    public void Handler_CanSuppressBuiltInCheckForProperty()
    {
        var instance = CreateFullyPopulated();
        instance.IntValue = 0; // would normally fail

        AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance,
            prop =>
            {
                if (prop.PropertyName == nameof(NonDefaultModel.IntValue))
                {
                    // zero is acceptable for this property in this test
                    prop.IsHandled = true;
                }
            });
    }

    [Fact]
    public void Handler_CanApplyStricterRuleAndFail()
    {
        var instance = CreateFullyPopulated();
        instance.StringValue = string.Empty; // passes the built-in check, but we want non-empty

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance,
                prop =>
                {
                    if (prop.PropertyName == nameof(NonDefaultModel.StringValue))
                    {
                        AssertThat.IsTrue(
                            string.IsNullOrEmpty((string?)prop.Value) == false,
                            "StringValue should not be empty");
                        prop.IsHandled = true;
                    }
                }));

        WriteLine(ex.Message);
        Assert.Contains("StringValue should not be empty", ex.Message);
    }

    [Fact]
    public void Handler_ExposesInstanceAndPropertyName()
    {
        var instance = CreateFullyPopulated();
        var seenNames = new System.Collections.Generic.List<string>();

        AssertThat.AllPropertiesAreNonNullAndNonDefaultValue(instance,
            prop =>
            {
                seenNames.Add(prop.PropertyName);
                Assert.Same(instance, prop.Instance);
            });

        Assert.Contains(nameof(NonDefaultModel.IntValue), seenNames);
        Assert.Contains(nameof(NonDefaultModel.StringValue), seenNames);
    }

    #endregion
}

public class NonDefaultModel
{
    public int IntValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
    public decimal DecimalValue { get; set; }
    public SampleStatus Status { get; set; }
    public Guid GuidValue { get; set; }
    public DateTime DateTimeValue { get; set; }
    public int? NullableInt { get; set; }
}
