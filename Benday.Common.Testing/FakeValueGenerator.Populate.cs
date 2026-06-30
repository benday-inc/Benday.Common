using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Benday.Common.Testing;

public static partial class FakeValueGenerator
{
    /// <summary>
    /// Maps a property type to a delegate that produces a fake value for it. This is the dispatch
    /// table that drives <see cref="PopulateFakeValues{T}"/>. Enum and nullable types are handled
    /// separately during dispatch and are intentionally not stored here.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Func<string, bool, int, object?>> _generators =
        new(new Dictionary<Type, Func<string, bool, int, object?>>
        {
            [typeof(int)] = (field, randomize, seed) => GetFakeValueForInt(field, randomize, seed),
            [typeof(long)] = (field, randomize, seed) => GetFakeValueForLong(field, randomize, seed),
            [typeof(double)] = (field, randomize, seed) => GetFakeValueForDouble(field, randomize, seed),
            [typeof(float)] = (field, randomize, seed) => GetFakeValueForFloat(field, randomize, seed),
            [typeof(decimal)] = (field, randomize, seed) => GetFakeValueForDecimal(field, randomize, seed),
            [typeof(bool)] = (field, randomize, seed) => GetFakeValueForBool(field, randomize, seed),
            [typeof(Guid)] = (field, randomize, seed) => GetFakeValueForGuid(field, randomize, seed),
            [typeof(string)] = (field, randomize, seed) => GetFakeValueForString(field, randomize, seed),
            [typeof(DateTime)] = (field, randomize, seed) => GetFakeValueForDateTime(field, randomize, seed),
            [typeof(byte[])] = (field, randomize, seed) => GetFakeValueForByteArray(field, randomize, seed),
        });

    /// <summary>
    /// Registers (or replaces) a generator for the supplied type so that <see cref="PopulateFakeValues{T}"/>
    /// can populate properties of that type. Use this to add support for types beyond the built-in set,
    /// such as a custom value object.
    /// </summary>
    /// <param name="type">The property type the generator produces values for.</param>
    /// <param name="generator">
    /// A delegate that accepts the field name, a randomize flag, and a seed value, and returns a fake
    /// value assignable to <paramref name="type"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> or <paramref name="generator"/> is null.</exception>
    public static void RegisterGenerator(Type type, Func<string, bool, int, object?> generator)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(generator);

        _generators[type] = generator;
    }

    /// <summary>
    /// Populates the public, writable properties of <paramref name="populateThis"/> with fake values
    /// using reflection. Properties named in <paramref name="skipPropertyNames"/> are left untouched,
    /// as are properties with no public setter or with a type that has no registered generator. The
    /// returned <see cref="FakeValueResult{T}"/> reports which properties were populated and which were skipped.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="populateThis">The object to populate.</param>
    /// <param name="skipPropertyNames">The names of properties to leave untouched (case-insensitive). Optional.</param>
    /// <param name="randomize">When true, generated values are random; otherwise they are predictable.</param>
    /// <param name="seedValue">A seed used to keep predictable values unique across a collection (e.g. the item index).</param>
    /// <returns>A result describing the populated instance and any skipped properties.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="populateThis"/> is null.</exception>
    public static FakeValueResult<T> PopulateFakeValues<T>(
        T populateThis,
        string[]? skipPropertyNames = null,
        bool randomize = false,
        int seedValue = 0)
    {
        return PopulateFakeValuesCore(populateThis, null, skipPropertyNames, randomize, seedValue);
    }

    /// <summary>
    /// Populates the public, writable properties of <paramref name="populateThis"/> with fake values,
    /// giving the caller a chance to populate special-case properties with custom logic.
    /// </summary>
    /// <remarks>
    /// For every property that is not in <paramref name="skipPropertyNames"/>,
    /// <paramref name="handleSpecialCases"/> is invoked with a <see cref="PropertyPopulation{T}"/>
    /// describing the property. The handler can set the property however it likes (the strongly typed
    /// instance is available via <see cref="PropertyPopulation{T}.Instance"/>) and then set
    /// <see cref="PropertyPopulation{T}.IsHandled"/> to <c>true</c> to take that property over. Handled
    /// properties are reported as populated and the built-in generation is skipped for them. This hook
    /// runs before the public-setter check, so it can also populate read-only properties (for example
    /// by adding items to a collection) or properties whose type has no registered generator.
    /// </remarks>
    /// <typeparam name="T">The type of the object to populate.</typeparam>
    /// <param name="populateThis">The object to populate.</param>
    /// <param name="handleSpecialCases">A callback invoked once per property to optionally populate it with custom logic.</param>
    /// <param name="skipPropertyNames">The names of properties to leave untouched (case-insensitive). Optional.</param>
    /// <param name="randomize">When true, generated values are random; otherwise they are predictable.</param>
    /// <param name="seedValue">A seed used to keep predictable values unique across a collection (e.g. the item index).</param>
    /// <returns>A result describing the populated instance and any skipped properties.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="populateThis"/> is null.</exception>
    public static FakeValueResult<T> PopulateFakeValues<T>(
        T populateThis,
        Action<PropertyPopulation<T>>? handleSpecialCases,
        string[]? skipPropertyNames = null,
        bool randomize = false,
        int seedValue = 0)
    {
        return PopulateFakeValuesCore(populateThis, handleSpecialCases, skipPropertyNames, randomize, seedValue);
    }

    private static FakeValueResult<T> PopulateFakeValuesCore<T>(
        T populateThis,
        Action<PropertyPopulation<T>>? handleSpecialCases,
        string[]? skipPropertyNames,
        bool randomize,
        int seedValue)
    {
        ArgumentNullException.ThrowIfNull(populateThis);

        var skip = new HashSet<string>(
            skipPropertyNames ?? Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);

        var populated = new List<string>();
        var skipped = new List<SkippedProperty>();

        var properties = populateThis.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            // skip indexers
            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            var typeName = property.PropertyType.FullName ?? property.PropertyType.Name;

            if (skip.Contains(property.Name))
            {
                skipped.Add(new SkippedProperty(property.Name, typeName, SkipReason.ExplicitlySkipped));
                continue;
            }

            if (handleSpecialCases is not null)
            {
                var context = new PropertyPopulation<T>(
                    populateThis, property.Name, property.PropertyType, randomize, seedValue);

                handleSpecialCases(context);

                if (context.IsHandled)
                {
                    populated.Add(property.Name);
                    continue;
                }
            }

            if (property.CanWrite == false ||
                property.SetMethod is null ||
                property.SetMethod.IsPublic == false)
            {
                skipped.Add(new SkippedProperty(property.Name, typeName, SkipReason.NoSetter));
                continue;
            }

            if (TryGenerateValue(property.PropertyType, property.Name, randomize, seedValue, out var value))
            {
                property.SetValue(populateThis, value);
                populated.Add(property.Name);
            }
            else
            {
                skipped.Add(new SkippedProperty(property.Name, typeName, SkipReason.UnsupportedType));
            }
        }

        return new FakeValueResult<T>(populateThis, populated, skipped);
    }

    private static bool TryGenerateValue(
        Type propertyType,
        string fieldName,
        bool randomize,
        int seedValue,
        out object? value)
    {
        // treat a nullable value type the same as its underlying type
        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (underlyingType.IsEnum)
        {
            value = GetFakeValueForEnum(underlyingType, fieldName, randomize, seedValue);
            return true;
        }

        if (_generators.TryGetValue(underlyingType, out var generator))
        {
            value = generator(fieldName, randomize, seedValue);
            return true;
        }

        value = null;
        return false;
    }
}
