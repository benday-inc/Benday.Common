using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.Common.Testing;

/// <summary>
/// Describes why a property was skipped while populating fake values.
/// </summary>
public enum SkipReason
{
    /// <summary>
    /// The property name was supplied in the caller's skip list.
    /// </summary>
    ExplicitlySkipped,

    /// <summary>
    /// The property has no accessible (public) setter.
    /// </summary>
    NoSetter,

    /// <summary>
    /// There is no registered generator for the property's type.
    /// </summary>
    UnsupportedType
}

/// <summary>
/// Describes a single property that was not populated with a fake value and the reason why.
/// </summary>
public class SkippedProperty
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkippedProperty"/> class.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="typeName">The display name of the property's type.</param>
    /// <param name="reason">The reason the property was skipped.</param>
    public SkippedProperty(string name, string typeName, SkipReason reason)
    {
        Name = name;
        TypeName = typeName;
        Reason = reason;
    }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the display name of the property's type.
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// Gets the reason the property was skipped.
    /// </summary>
    public SkipReason Reason { get; }
}

/// <summary>
/// Represents the outcome of populating an object with fake values. Exposes the populated
/// instance along with which properties were populated and which were skipped (and why).
/// </summary>
/// <typeparam name="T">The type of the populated object.</typeparam>
public class FakeValueResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeValueResult{T}"/> class.
    /// </summary>
    /// <param name="instance">The populated object.</param>
    /// <param name="populatedProperties">The names of the properties that were populated with a fake value.</param>
    /// <param name="skippedProperties">The properties that were not populated, along with the reason for each.</param>
    public FakeValueResult(
        T instance,
        IReadOnlyList<string> populatedProperties,
        IReadOnlyList<SkippedProperty> skippedProperties)
    {
        Instance = instance;
        PopulatedProperties = populatedProperties;
        SkippedProperties = skippedProperties;
    }

    /// <summary>
    /// Gets the populated object.
    /// </summary>
    public T Instance { get; }

    /// <summary>
    /// Gets the names of the properties that were populated with a fake value.
    /// </summary>
    public IReadOnlyList<string> PopulatedProperties { get; }

    /// <summary>
    /// Gets the properties that were not populated, along with the reason for each.
    /// </summary>
    public IReadOnlyList<SkippedProperty> SkippedProperties { get; }

    /// <summary>
    /// Asserts that every property was either populated with a fake value or explicitly skipped by
    /// the caller. Fails if any property was skipped because it had no setter or no registered
    /// generator -- i.e. a property silently escaped without being exercised.
    /// </summary>
    /// <param name="message">An optional message to prepend to the auto-generated failure message.</param>
    /// <returns>This result, to allow chaining.</returns>
    /// <exception cref="AssertionException">Thrown when one or more properties were neither populated nor explicitly skipped.</exception>
    public FakeValueResult<T> AssertAllPropertiesAccountedFor(string? message = null)
    {
        var offenders = SkippedProperties
            .Where(x => x.Reason != SkipReason.ExplicitlySkipped)
            .ToList();

        if (offenders.Count > 0)
        {
            throw new AssertionException(
                BuildFailureMessage(
                    nameof(AssertAllPropertiesAccountedFor),
                    "neither populated nor explicitly skipped",
                    offenders,
                    message));
        }

        return this;
    }

    /// <summary>
    /// Asserts that no property was skipped because its type had no registered generator. Properties
    /// that were explicitly skipped or have no setter do not cause a failure.
    /// </summary>
    /// <param name="message">An optional message to prepend to the auto-generated failure message.</param>
    /// <returns>This result, to allow chaining.</returns>
    /// <exception cref="AssertionException">Thrown when one or more properties had an unsupported type.</exception>
    public FakeValueResult<T> AssertNoUnsupportedProperties(string? message = null)
    {
        var offenders = SkippedProperties
            .Where(x => x.Reason == SkipReason.UnsupportedType)
            .ToList();

        if (offenders.Count > 0)
        {
            throw new AssertionException(
                BuildFailureMessage(
                    nameof(AssertNoUnsupportedProperties),
                    "had no registered generator",
                    offenders,
                    message));
        }

        return this;
    }

    private static string BuildFailureMessage(
        string assertName,
        string problemDescription,
        IReadOnlyList<SkippedProperty> offenders,
        string? message)
    {
        var builder = new StringBuilder();

        if (string.IsNullOrWhiteSpace(message) == false)
        {
            builder.AppendLine(message);
        }

        builder.AppendLine($"Assert.{assertName} failed:");
        builder.AppendLine($"{offenders.Count} {(offenders.Count == 1 ? "property was" : "properties were")} {problemDescription}:");

        foreach (var offender in offenders)
        {
            builder.AppendLine($"  - {offender.Name} ({offender.TypeName}) [{offender.Reason}]");
        }

        builder.Append("Add a generator for these types (FakeValueGenerator.RegisterGenerator) or pass the property names in skipPropertyNames.");

        return builder.ToString();
    }
}
