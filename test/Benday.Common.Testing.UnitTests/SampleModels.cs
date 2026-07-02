using System;

namespace Benday.Common.Testing.UnitTests;

public enum SampleStatus
{
    Unknown,
    Active,
    Inactive
}

/// <summary>
/// Every property is a public, writable type that has a built-in generator.
/// </summary>
public class FullySupportedModel
{
    public int IntValue { get; set; }
    public long LongValue { get; set; }
    public double DoubleValue { get; set; }
    public float FloatValue { get; set; }
    public decimal DecimalValue { get; set; }
    public bool BoolValue { get; set; }
    public Guid GuidValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
    public DateTime DateTimeValue { get; set; }
    public byte[] ByteArrayValue { get; set; } = Array.Empty<byte>();
    public SampleStatus Status { get; set; }
    public int? NullableInt { get; set; }
    public SampleStatus? NullableStatus { get; set; }
}

/// <summary>
/// Mixes a populatable property with a read-only property and a property whose
/// type has no registered generator.
/// </summary>
public class MixedModel
{
    public int IntValue { get; set; }
    public string ReadOnlyValue { get; } = "readonly";
    public CustomThing? Custom { get; set; }
}

public class CustomThing
{
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Uses a type that no other test references so that registering a generator for it
/// does not interfere with the unsupported-type tests when classes run in parallel.
/// </summary>
public class RegisterModel
{
    public RegisterOnlyThing? Thing { get; set; }
}

public class RegisterOnlyThing
{
    public string Value { get; set; } = string.Empty;
}
