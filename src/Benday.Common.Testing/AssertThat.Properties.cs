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
    /// Single-dimension arrays (such as <c>byte[]</c>, the common rowversion / timestamp case) are
    /// compared element-by-element when their element type is itself reliably comparable.
    /// Reference-type properties that do not provide value equality cannot be compared reliably; rather
    /// than silently comparing them by reference, they are reported as failures so they are never passed
    /// over unnoticed. Add such properties to <paramref name="skipPropertyNames"/> (or give the type
    /// value equality) to handle them explicitly. Indexers and properties without a public getter are
    /// ignored.
    /// </remarks>
    /// <typeparam name="T">The declared type of the objects to compare.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="skipPropertyNames">
    /// The names of properties to skip the built-in comparison for (case-insensitive). On this overload
    /// there is no handler, so a skipped property is simply not compared. Optional.
    /// </param>
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
    /// For every property -- including those named in <paramref name="skipPropertyNames"/> --
    /// <paramref name="handleSpecialCases"/> is invoked with a <see cref="PropertyComparison{T}"/>
    /// describing the property and both values. The handler can call
    /// <see cref="PropertyComparison{T}.HandleManuallyPass"/> or
    /// <see cref="PropertyComparison{T}.HandleManuallyFail(string)"/> to report a custom result that is
    /// folded into the aggregated failure message, or set
    /// <see cref="PropertyComparison{T}.IsHandled"/> to <c>true</c> and throw (for example via
    /// <see cref="Fail(string)"/>) to fail fast. Either way, marking the property handled suppresses the
    /// built-in equality check. Properties the handler leaves unhandled fall through to the default
    /// comparison described on the other overload -- unless they are named in
    /// <paramref name="skipPropertyNames"/>, in which case the built-in comparison is skipped and the
    /// property is left uncompared.
    /// </remarks>
    /// <typeparam name="T">The declared type of the objects to compare.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="handleSpecialCases">A callback invoked once per property to optionally override the comparison.</param>
    /// <param name="skipPropertyNames">
    /// The names of properties to skip the built-in comparison for (case-insensitive). Skipped
    /// properties are still passed to <paramref name="handleSpecialCases"/>, so they can be compared
    /// with custom logic; a skipped property the handler does not handle is left uncompared. Optional.
    /// </param>
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

        foreach (var property in GetAssertableProperties(expectedType))
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
                    if (context.HasManualFailure)
                    {
                        mismatches.Add($"  - {property.Name}: {context.ManualFailureMessage}");
                    }

                    continue;
                }
            }

            // Properties named in skipPropertyNames are excluded from the built-in comparison, but
            // were still offered to handleSpecialCases above so they can be compared with custom logic.
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

            if (ValuesAreEqual(property.PropertyType, expectedValue, actualValue) == false)
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
    /// An optional callback invoked once per property -- including those named in
    /// <paramref name="skipPropertyNames"/>. The handler can call
    /// <see cref="PropertyAssert{T}.HandleManuallyPass"/> or
    /// <see cref="PropertyAssert{T}.HandleManuallyFail(string)"/> to report a custom result that is
    /// folded into the aggregated failure message, or set <see cref="PropertyAssert{T}.IsHandled"/> to
    /// <c>true</c> and throw to fail fast. Either way, marking the property handled suppresses the
    /// built-in non-default check.
    /// </param>
    /// <param name="skipPropertyNames">
    /// The names of properties to skip the built-in non-default check for (case-insensitive). Skipped
    /// properties are still passed to <paramref name="handleSpecialCases"/>, so they can be checked
    /// with custom logic; a skipped property the handler does not handle is left unchecked. Optional.
    /// </param>
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

        foreach (var property in GetAssertableProperties(instanceType))
        {
            var value = property.GetValue(instance);

            if (handleSpecialCases is not null)
            {
                var context = new PropertyAssert<T>(
                    instance, property.Name, property.PropertyType, value);

                handleSpecialCases(context);

                if (context.IsHandled)
                {
                    if (context.HasManualFailure)
                    {
                        offenders.Add($"  - {property.Name}: {context.ManualFailureMessage}");
                    }

                    continue;
                }
            }

            // Properties named in skipPropertyNames are excluded from the built-in check, but were
            // still offered to handleSpecialCases above so they can be checked with custom logic.
            if (skip.Contains(property.Name))
            {
                continue;
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
    /// Enumerates the public, readable, non-indexer instance properties of a type. Properties named in
    /// <c>skipPropertyNames</c> are intentionally not filtered here: callers offer every property to the
    /// <c>handleSpecialCases</c> callback and suppress only the built-in check for skipped properties.
    /// </summary>
    private static IEnumerable<PropertyInfo> GetAssertableProperties(Type type)
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

            yield return property;
        }
    }

    private static object? GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    /// <summary>
    /// Determines whether values of the supplied type can be compared by value. Value types (including
    /// enums and structs), strings, single-dimension arrays whose element type is itself reliably
    /// comparable (so <c>byte[]</c>, <c>int[]</c>, <c>string[]</c>, and the like), and reference types
    /// that implement <see cref="IEquatable{T}"/> or override <see cref="object.Equals(object)"/> are
    /// considered reliably comparable. Other reference types are not, because comparing them would fall
    /// back to reference equality.
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

        if (underlyingType.IsArray && underlyingType.GetArrayRank() == 1)
        {
            var elementType = underlyingType.GetElementType();

            return elementType is not null && IsReliablyComparable(elementType);
        }

        if (typeof(IEquatable<>).MakeGenericType(underlyingType).IsAssignableFrom(underlyingType))
        {
            return true;
        }

        var equalsMethod = underlyingType.GetMethod(
            nameof(Equals), BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(object) }, null);

        return equalsMethod is not null && equalsMethod.DeclaringType != typeof(object);
    }

    /// <summary>
    /// Compares two property values by value. Single-dimension arrays are compared element-by-element so
    /// that types such as <c>byte[]</c> (the common rowversion / timestamp case) are compared by content
    /// rather than by reference; everything else uses <see cref="EqualityComparer{T}.Default"/>.
    /// </summary>
    private static bool ValuesAreEqual(Type type, object? expected, object? actual)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        if (underlyingType.IsArray && underlyingType.GetArrayRank() == 1)
        {
            return ArraysAreEqual(expected as Array, actual as Array);
        }

        return EqualityComparer<object>.Default.Equals(expected, actual);
    }

    private static bool ArraysAreEqual(Array? expected, Array? actual)
    {
        if (expected is null && actual is null)
        {
            return true;
        }

        if (expected is null || actual is null)
        {
            return false;
        }

        if (expected.Length != actual.Length)
        {
            return false;
        }

        for (var i = 0; i < expected.Length; i++)
        {
            if (EqualityComparer<object>.Default.Equals(expected.GetValue(i), actual.GetValue(i)) == false)
            {
                return false;
            }
        }

        return true;
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
