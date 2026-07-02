using System;

namespace Benday.Common.Testing.UnitTests.Assertions;

public class PropertiesAreEqualFixture : TestClassBase
{
    public PropertiesAreEqualFixture(ITestOutputHelper output) : base(output)
    {
    }

    private static ComparableModel CreateSample()
    {
        return new ComparableModel
        {
            IntValue = 42,
            StringValue = "hello",
            DecimalValue = 12.5m,
            Status = SampleStatus.Active,
            NullableInt = 7,
            DateTimeValue = new DateTime(1999, 1, 1),
        };
    }

    [Fact]
    public void IdenticalObjects_DoNotThrow()
    {
        var expected = CreateSample();
        var actual = CreateSample();

        AssertThat.PropertiesAreEqual(expected, actual);
    }

    [Fact]
    public void BothNull_DoNotThrow()
    {
        AssertThat.PropertiesAreEqual<ComparableModel>(null!, null!);
    }

    [Fact]
    public void ExpectedNullActualNotNull_Throws()
    {
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual<ComparableModel>(null!, CreateSample()));

        WriteLine(ex.Message);
        Assert.Contains("Assert.PropertiesAreEqual failed", ex.Message);
    }

    [Fact]
    public void SingleMismatch_ThrowsAndNamesTheProperty()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        actual.IntValue = 99;

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("1 property did not match", ex.Message);
        Assert.Contains("IntValue", ex.Message);
        Assert.DoesNotContain("StringValue", ex.Message);
    }

    [Fact]
    public void MultipleMismatches_AreAllReported()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        actual.IntValue = 99;
        actual.StringValue = "different";
        actual.Status = SampleStatus.Inactive;

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("3 properties did not match", ex.Message);
        Assert.Contains("IntValue", ex.Message);
        Assert.Contains("StringValue", ex.Message);
        Assert.Contains("Status", ex.Message);
    }

    [Fact]
    public void SkippedProperty_IsNotCompared()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        actual.IntValue = 99;

        AssertThat.PropertiesAreEqual(expected, actual, skipPropertyNames: new[] { "intvalue" });
    }

    [Fact]
    public void CustomMessage_IsIncludedInFailure()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        actual.IntValue = 99;

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual, message: "Round-trip should preserve values"));

        WriteLine(ex.Message);
        Assert.Contains("Round-trip should preserve values", ex.Message);
    }

    [Fact]
    public void DifferentRuntimeTypes_Throw()
    {
        object expected = CreateSample();
        object actual = new ComparableSubclass();

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("type", ex.Message);
    }

    [Fact]
    public void NullableValueProperties_CompareByValue()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        expected.NullableInt = null;
        actual.NullableInt = null;

        AssertThat.PropertiesAreEqual(expected, actual);
    }

    [Fact]
    public void UncomparableReferenceProperty_IsReportedNotSilentlyPassed()
    {
        // Two distinct instances with identical contents; UncomparableThing has no value equality.
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("could not be compared reliably", ex.Message);
        Assert.Contains("Thing", ex.Message);
    }

    [Fact]
    public void UncomparableProperty_CanBeSkipped()
    {
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "y" },
        };

        AssertThat.PropertiesAreEqual(expected, actual, skipPropertyNames: new[] { "Thing" });
    }

    [Fact]
    public void ReferenceTypeWithValueEquality_IsComparedByValue()
    {
        var expected = new ModelWithEquatable
        {
            IntValue = 1,
            Thing = new EquatableThing { Value = "same" },
        };
        var actual = new ModelWithEquatable
        {
            IntValue = 1,
            Thing = new EquatableThing { Value = "same" },
        };

        AssertThat.PropertiesAreEqual(expected, actual);
    }

    [Fact]
    public void ReferenceTypeWithValueEquality_DetectsDifference()
    {
        var expected = new ModelWithEquatable
        {
            IntValue = 1,
            Thing = new EquatableThing { Value = "a" },
        };
        var actual = new ModelWithEquatable
        {
            IntValue = 1,
            Thing = new EquatableThing { Value = "b" },
        };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("Thing", ex.Message);
        Assert.Contains("did not match", ex.Message);
    }

    #region arrays

    [Fact]
    public void ByteArrayProperty_EqualContents_DoNotThrow()
    {
        // distinct instances, identical contents -- the rowversion / timestamp case
        var expected = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3, 4 } };
        var actual = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3, 4 } };

        AssertThat.PropertiesAreEqual(expected, actual);
    }

    [Fact]
    public void ByteArrayProperty_DifferentContents_Throws()
    {
        var expected = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3, 4 } };
        var actual = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3, 9 } };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("Timestamp", ex.Message);
        Assert.Contains("did not match", ex.Message);
        Assert.DoesNotContain("could not be compared reliably", ex.Message);
    }

    [Fact]
    public void ByteArrayProperty_DifferentLengths_Throws()
    {
        var expected = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3, 4 } };
        var actual = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3 } };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("Timestamp", ex.Message);
    }

    [Fact]
    public void ByteArrayProperty_BothNull_DoNotThrow()
    {
        var expected = new ModelWithArrays { Timestamp = null };
        var actual = new ModelWithArrays { Timestamp = null };

        AssertThat.PropertiesAreEqual(expected, actual);
    }

    [Fact]
    public void ByteArrayProperty_OneNull_Throws()
    {
        var expected = new ModelWithArrays { Timestamp = new byte[] { 1, 2, 3 } };
        var actual = new ModelWithArrays { Timestamp = null };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("Timestamp", ex.Message);
    }

    [Fact]
    public void StringArrayProperty_ComparedByContent()
    {
        var expected = new ModelWithArrays { Tags = new[] { "a", "b" } };
        var actual = new ModelWithArrays { Tags = new[] { "a", "b" } };

        AssertThat.PropertiesAreEqual(expected, actual);
    }

    [Fact]
    public void ArrayOfUncomparableElements_IsStillReportedAsUncomparable()
    {
        // element type has no value equality, so the array itself is not reliably comparable
        var expected = new ModelWithUncomparableArray
        {
            Things = new[] { new UncomparableThing { Value = "x" } },
        };
        var actual = new ModelWithUncomparableArray
        {
            Things = new[] { new UncomparableThing { Value = "x" } },
        };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual));

        WriteLine(ex.Message);
        Assert.Contains("could not be compared reliably", ex.Message);
        Assert.Contains("Things", ex.Message);
    }

    #endregion

    #region special-case lambda

    [Fact]
    public void Handler_CanCustomizeComparisonOfUncomparableProperty()
    {
        // Two distinct UncomparableThing instances with identical content. Built-in comparison would
        // report it as uncomparable; the handler compares the meaningful field instead.
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };

        AssertThat.PropertiesAreEqual(expected, actual,
            prop =>
            {
                if (prop.PropertyName == nameof(ModelWithUncomparable.Thing))
                {
                    var expectedThing = (UncomparableThing?)prop.ExpectedValue;
                    var actualThing = (UncomparableThing?)prop.ActualValue;
                    AssertThat.AreEqual(expectedThing?.Value, actualThing?.Value, "Thing.Value");
                    prop.IsHandled = true;
                }
            });
    }

    [Fact]
    public void Handler_CustomComparisonCanFail()
    {
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "different" },
        };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual,
                prop =>
                {
                    if (prop.PropertyName == nameof(ModelWithUncomparable.Thing))
                    {
                        var expectedThing = (UncomparableThing?)prop.ExpectedValue;
                        var actualThing = (UncomparableThing?)prop.ActualValue;
                        AssertThat.AreEqual(expectedThing?.Value, actualThing?.Value, "Thing.Value");
                        prop.IsHandled = true;
                    }
                }));

        WriteLine(ex.Message);
        Assert.Contains("Thing.Value", ex.Message);
    }

    [Fact]
    public void Handler_UnhandledPropertiesStillUseBuiltInComparison()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        actual.IntValue = 99;

        // handler only inspects, never sets IsHandled, so IntValue still fails via the built-in check
        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual, prop => { /* observe only */ }));

        WriteLine(ex.Message);
        Assert.Contains("IntValue", ex.Message);
    }

    [Fact]
    public void Handler_FiresForSkippedProperties()
    {
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };

        var sawThing = false;

        // Thing is skipped (no built-in compare) but the handler still gets to see and pass it.
        AssertThat.PropertiesAreEqual(expected, actual,
            prop =>
            {
                if (prop.PropertyName == nameof(ModelWithUncomparable.Thing))
                {
                    sawThing = true;
                    var e = (UncomparableThing?)prop.ExpectedValue;
                    var a = (UncomparableThing?)prop.ActualValue;
                    if (e?.Value == a?.Value)
                    {
                        prop.HandleManuallyPass();
                    }
                    else
                    {
                        prop.HandleManuallyFail($"Thing.Value mismatch: '{e?.Value}' vs '{a?.Value}'");
                    }
                }
            },
            skipPropertyNames: new[] { nameof(ModelWithUncomparable.Thing) });

        Assert.True(sawThing, "handler should fire for skipped properties");
    }

    [Fact]
    public void Handler_ManualFailIsFoldedIntoAggregatedReport()
    {
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 99, // also wrong via the built-in compare
            Thing = new UncomparableThing { Value = "y" },
        };

        var ex = Assert.Throws<AssertionException>(() =>
            AssertThat.PropertiesAreEqual(expected, actual,
                prop =>
                {
                    if (prop.PropertyName == nameof(ModelWithUncomparable.Thing))
                    {
                        var e = (UncomparableThing?)prop.ExpectedValue;
                        var a = (UncomparableThing?)prop.ActualValue;
                        prop.HandleManuallyFail($"Thing.Value mismatch: '{e?.Value}' vs '{a?.Value}'");
                    }
                },
                skipPropertyNames: new[] { nameof(ModelWithUncomparable.Thing) }));

        WriteLine(ex.Message);
        // both the manual failure and the built-in IntValue mismatch are reported together
        Assert.Contains("2 properties did not match", ex.Message);
        Assert.Contains("Thing.Value mismatch", ex.Message);
        Assert.Contains("IntValue", ex.Message);
    }

    [Fact]
    public void Handler_ManualPassSuppressesBuiltInComparison()
    {
        var expected = CreateSample();
        var actual = CreateSample();
        actual.IntValue = 99; // would fail the built-in compare

        AssertThat.PropertiesAreEqual(expected, actual,
            prop =>
            {
                if (prop.PropertyName == nameof(ComparableModel.IntValue))
                {
                    prop.HandleManuallyPass();
                }
            });
    }

    [Fact]
    public void Handler_SkippedAndUnhandledPropertyIsLeftUncompared()
    {
        var expected = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "x" },
        };
        var actual = new ModelWithUncomparable
        {
            IntValue = 1,
            Thing = new UncomparableThing { Value = "different" },
        };

        // Thing is skipped and the handler never handles it -> no failure, no "uncomparable" report
        AssertThat.PropertiesAreEqual(expected, actual,
            prop => { /* observe only */ },
            skipPropertyNames: new[] { nameof(ModelWithUncomparable.Thing) });
    }

    #endregion
}

public class ComparableModel
{
    public int IntValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
    public decimal DecimalValue { get; set; }
    public SampleStatus Status { get; set; }
    public int? NullableInt { get; set; }
    public DateTime DateTimeValue { get; set; }

    // a get-only property is still compared (here it is a constant, so it never adds noise);
    // the indexer below must be ignored by the comparison
    public string Computed => "constant";

    public string this[int index] => StringValue;
}

public class ComparableSubclass : ComparableModel
{
}

public class ModelWithUncomparable
{
    public int IntValue { get; set; }
    public UncomparableThing? Thing { get; set; }
}

public class UncomparableThing
{
    public string Value { get; set; } = string.Empty;
}

public class ModelWithArrays
{
    public byte[]? Timestamp { get; set; }
    public string[]? Tags { get; set; }
}

public class ModelWithUncomparableArray
{
    public UncomparableThing[]? Things { get; set; }
}

public class ModelWithEquatable
{
    public int IntValue { get; set; }
    public EquatableThing? Thing { get; set; }
}

public class EquatableThing : IEquatable<EquatableThing>
{
    public string Value { get; set; } = string.Empty;

    public bool Equals(EquatableThing? other)
    {
        return other is not null && other.Value == Value;
    }

    public override bool Equals(object? obj) => Equals(obj as EquatableThing);

    public override int GetHashCode() => Value.GetHashCode();
}
