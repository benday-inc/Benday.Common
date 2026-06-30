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
