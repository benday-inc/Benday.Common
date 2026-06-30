using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Benday.Common.Testing;

public static partial class AssertThat
{
    /// <summary>
    /// Verifies that two objects are equal by comparing each of their public, readable properties
    /// using reflection. Every mismatch is collected and reported together rather than failing on
    /// the first difference. This is the companion to <see cref="FakeValueGenerator.PopulateFakeValues{T}(T, string[], bool, int)"/>
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
        PropertiesAreEqualCore(expected, actual, null, skipPropertyNames, message);
    }

    /// <summary>
    /// Verifies that two objects are equal by comparing each of their public, readable properties,
    /// giving the caller a chance to handle special-case properties with a custom check.
    /// </summary>
    /// <remarks>
    /// For every property, <paramref name="handleSpecialCases"/> is invoked with a
    /// <see cref="PropertyComparison{T}"/> describing the property and both values. If the handler sets
    /// <see cref="PropertyComparison{T}.IsHandled"/> to <c>true</c>, the built-in equality check is
    /// skipped for that property; the handler is expected to throw (for example via
    /// <see cref="Fail(string)"/>) when its own comparison fails. Properties the handler leaves
    /// unhandled fall through to the default comparison described on the other overload.
    /// </remarks>
    /// <typeparam name="T">The declared type of the objects to compare.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="handleSpecialCases">A callback invoked once per property to optionally override the comparison.</param>
    /// <param name="skipPropertyNames">The names of properties to ignore entirely (case-insensitive). Optional.</param>
    /// <param name="message">An optional message to prepend to the auto-generated failure message.</param>
    /// <exception cref="AssertionException">
    /// Thrown when one or more properties differ, when exactly one of the objects is null, when the
    /// objects are of different runtime types, or when a property cannot be compared reliably.
    /// </exception>
    public static void PropertiesAreEqual<T>(
        T expected,
        T actual,
        Action<PropertyComparison<T>>? handleSpecialCases,
        string[]? skipPropertyNames = null,
        string? message = null)
    {
        PropertiesAreEqualCore(expected, actual, handleSpecialCases, skipPropertyNames, message);
    }

    private static void PropertiesAreEqualCore<T>(
        T expected,
        T actual,
        Action<PropertyComparison<T>>? handleSpecialCases,
        string[]? skipPropertyNames,
        string? message)
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

        var skip = BuildSkipSet(skipPropertyNames);

        var mismatches = new List<string>();
        var uncomparable = new List<string>();

        foreach (var property in GetAssertableProperties(expectedType, skip))
        {
            var expectedValue = property.GetValue(expected);
            var actualValue = property.GetValue(actual);

            if (handleSpecialCases is not null)
            {
                var context = new PropertyComparison<T>(
                    expected, actual, property.Name, property.PropertyType, expectedValue, actualValue);

                handleSpecialCases(context);

                if (context.IsHandled)
                {
                    continue;
                }
            }

            if (IsReliablyComparable(property.PropertyType) == false)
            {
                uncomparable.Add(
                    $"  - {property.Name} ({AssertionMessageFormatter.GetTypeName(property.PropertyType)})");
                continue;
            }

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
    /// Verifies that every public, readable property of <paramref name="instance"/> holds a value
    /// other than the default for its type. This is the natural follow-up to
    /// <see cref="FakeValueGenerator.PopulateFakeValues{T}(T, string[], bool, int)"/>: it confirms
    /// that nothing was left unset.
    /// </summary>
    /// <remarks>
    /// A property is considered unset when its value equals <c>default(TProperty)</c> -- that is,
    /// <c>null</c> for reference types and nullable value types, and the zero value (<c>0</c>,
    /// <c>false</c>, <see cref="Guid.Empty"/>, <see cref="DateTime.MinValue"/>, and so on) for value
    /// types. Empty-but-non-null values such as an empty string or empty array are not treated as
    /// defaults; use <paramref name="handleSpecialCases"/> to apply stricter per-property rules.
    /// Indexers and properties without a public getter are ignored.
    /// </remarks>
    /// <typeparam name="T">The type of the object to check.</typeparam>
    /// <param name="instance">The object whose properties are checked.</param>
    /// <param name="handleSpecialCases">
    /// An optional callback invoked once per property. Setting
    /// <see cref="PropertyAssert{T}.IsHandled"/> to <c>true</c> suppresses the built-in non-default
    /// check for that property; the handler is expected to throw when its own check fails.
    /// </param>
    /// <param name="skipPropertyNames">The names of properties to ignore entirely (case-insensitive). Optional.</param>
    /// <param name="message">An optional message to prepend to the auto-generated failure message.</param>
    /// <exception cref="AssertionException">Thrown when <paramref name="instance"/> is null or one or more properties hold a default value.</exception>
    public static void AllPropertiesAreNonNullAndNonDefaultValue<T>(
        T instance,
        Action<PropertyAssert<T>>? handleSpecialCases = null,
        string[]? skipPropertyNames = null,
        string? message = null)
    {
        if (instance is null)
        {
            throw new AssertionException(
                AssertionMessageFormatter.FormatSimpleMessage(
                    message ?? string.Empty,
                    "AllPropertiesAreNonNullAndNonDefaultValue",
                    "Expected a non-null instance, but value was null"));
        }

        var skip = BuildSkipSet(skipPropertyNames);
        var instanceType = instance.GetType();

        var offenders = new List<string>();

        foreach (var property in GetAssertableProperties(instanceType, skip))
        {
            var value = property.GetValue(instance);

            if (handleSpecialCases is not null)
            {
                var context = new PropertyAssert<T>(
                    instance, property.Name, property.PropertyType, value);

                handleSpecialCases(context);

                if (context.IsHandled)
                {
                    continue;
                }
            }

            var defaultValue = GetDefaultValue(property.PropertyType);

            if (EqualityComparer<object>.Default.Equals(value, defaultValue))
            {
                offenders.Add(
                    $"  - {property.Name} ({AssertionMessageFormatter.GetTypeName(property.PropertyType)}): " +
                    $"{AssertionMessageFormatter.FormatValue(value)}");
            }
        }

        if (offenders.Count == 0)
        {
            return;
        }

        var builder = new StringBuilder();

        if (string.IsNullOrWhiteSpace(message) == false)
        {
            builder.AppendLine(message);
        }

        builder.AppendLine("Assert.AllPropertiesAreNonNullAndNonDefaultValue failed.");
        builder.AppendLine(
            $"{offenders.Count} {(offenders.Count == 1 ? "property was" : "properties were")} null or set to a default value:");

        foreach (var offender in offenders)
        {
            builder.AppendLine(offender);
        }

        throw new AssertionException(builder.ToString().TrimEnd());
    }

    private static HashSet<string> BuildSkipSet(string[]? skipPropertyNames)
    {
        return new HashSet<string>(
            skipPropertyNames ?? Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Enumerates the public, readable, non-indexer instance properties of a type, excluding any
    /// whose name appears in the supplied skip set.
    /// </summary>
    private static IEnumerable<PropertyInfo> GetAssertableProperties(Type type, HashSet<string> skip)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

            yield return property;
        }
    }

    private static object? GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
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
