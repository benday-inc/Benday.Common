using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Benday.Common.Testing;

public static partial class AssertThat
{
    /// <summary>
    /// Verifies that two objects are equal by comparing each of their public, readable properties
    /// using reflection. Every mismatch is collected and reported together rather than failing on
    /// the first difference. This is the companion to <see cref="FakeValueGenerator.PopulateFakeValues{T}"/>
    /// and removes the need for hand-written, property-by-property comparison helpers in tests.
    /// </summary>
    /// <remarks>
    /// Properties are compared with <see cref="EqualityComparer{T}.Default"/>, so types that override
    /// <see cref="object.Equals(object)"/> or implement <see cref="IEquatable{T}"/> compare by value.
    /// Reference-type properties that do not provide value equality cannot be compared reliably; rather
    /// than silently comparing them by reference, they are reported as failures so they are never passed
    /// over unnoticed. Add such properties to <paramref name="skipPropertyNames"/> (or give the type
    /// value equality) to handle them explicitly. Indexers and properties without a public getter are
    /// ignored.
    /// </remarks>
    /// <typeparam name="T">The declared type of the objects to compare.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="skipPropertyNames">The names of properties to ignore (case-insensitive). Optional.</param>
    /// <param name="message">An optional message to prepend to the auto-generated failure message.</param>
    /// <exception cref="AssertionException">
    /// Thrown when one or more properties differ, when exactly one of the objects is null, when the
    /// objects are of different runtime types, or when a property cannot be compared reliably.
    /// </exception>
    public static void PropertiesAreEqual<T>(
        T expected,
        T actual,
        string[]? skipPropertyNames = null,
        string? message = null)
    {
        if (expected is null && actual is null)
        {
            return;
        }

        if (expected is null || actual is null)
        {
            throw new AssertionException(
                AssertionMessageFormatter.FormatComparisonMessage(
                    expected, actual, message ?? string.Empty, "PropertiesAreEqual"));
        }

        var expectedType = expected.GetType();
        var actualType = actual.GetType();

        if (expectedType != actualType)
        {
            throw new AssertionException(
                AssertionMessageFormatter.FormatSimpleMessage(
                    message ?? string.Empty, "PropertiesAreEqual",
                    $"Expected type: {AssertionMessageFormatter.GetTypeName(expectedType)}, " +
                    $"Actual type: {AssertionMessageFormatter.GetTypeName(actualType)}"));
        }

        var skip = new HashSet<string>(
            skipPropertyNames ?? Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);

        var mismatches = new List<string>();
        var uncomparable = new List<string>();

        var properties = expectedType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            // skip indexers
            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (property.CanRead == false ||
                property.GetMethod is null ||
                property.GetMethod.IsPublic == false)
            {
                continue;
            }

            if (skip.Contains(property.Name))
            {
                continue;
            }

            if (IsReliablyComparable(property.PropertyType) == false)
            {
                uncomparable.Add(
                    $"  - {property.Name} ({AssertionMessageFormatter.GetTypeName(property.PropertyType)})");
                continue;
            }

            var expectedValue = property.GetValue(expected);
            var actualValue = property.GetValue(actual);

            if (EqualityComparer<object>.Default.Equals(expectedValue, actualValue) == false)
            {
                mismatches.Add(
                    $"  - {property.Name}: expected {AssertionMessageFormatter.FormatValue(expectedValue)}, " +
                    $"actual {AssertionMessageFormatter.FormatValue(actualValue)}");
            }
        }

        if (mismatches.Count == 0 && uncomparable.Count == 0)
        {
            return;
        }

        throw new AssertionException(
            BuildPropertiesFailureMessage(message, mismatches, uncomparable));
    }

    /// <summary>
    /// Determines whether values of the supplied type can be compared by value. Value types (including
    /// enums and structs), strings, and reference types that implement <see cref="IEquatable{T}"/> or
    /// override <see cref="object.Equals(object)"/> are considered reliably comparable. Other reference
    /// types are not, because comparing them would fall back to reference equality.
    /// </summary>
    private static bool IsReliablyComparable(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        if (underlyingType.IsValueType)
        {
            return true;
        }

        if (underlyingType == typeof(string))
        {
            return true;
        }

        if (typeof(IEquatable<>).MakeGenericType(underlyingType).IsAssignableFrom(underlyingType))
        {
            return true;
        }

        var equalsMethod = underlyingType.GetMethod(
            nameof(Equals), BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(object) }, null);

        return equalsMethod is not null && equalsMethod.DeclaringType != typeof(object);
    }

    private static string BuildPropertiesFailureMessage(
        string? message,
        IReadOnlyList<string> mismatches,
        IReadOnlyList<string> uncomparable)
    {
        var builder = new StringBuilder();

        if (string.IsNullOrWhiteSpace(message) == false)
        {
            builder.AppendLine(message);
        }

        builder.AppendLine("Assert.PropertiesAreEqual failed.");

        if (mismatches.Count > 0)
        {
            builder.AppendLine(
                $"{mismatches.Count} {(mismatches.Count == 1 ? "property" : "properties")} did not match:");

            foreach (var mismatch in mismatches)
            {
                builder.AppendLine(mismatch);
            }
        }

        if (uncomparable.Count > 0)
        {
            builder.AppendLine(
                $"{uncomparable.Count} {(uncomparable.Count == 1 ? "property" : "properties")} could not be compared reliably (no value equality):");

            foreach (var item in uncomparable)
            {
                builder.AppendLine(item);
            }

            builder.AppendLine(
                "Pass these property names in skipPropertyNames or implement IEquatable<T> / override Equals on the type.");
        }

        return builder.ToString().TrimEnd();
    }
}
