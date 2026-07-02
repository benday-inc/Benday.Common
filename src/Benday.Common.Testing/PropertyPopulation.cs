using System;

namespace Benday.Common.Testing;

/// <summary>
/// Supplies the context for a single property while
/// <see cref="FakeValueGenerator.PopulateFakeValues{T}(T, Action{PropertyPopulation{T}}, string[], bool, int)"/>
/// populates an object with fake values. A test author can populate the property with custom logic
/// (the strongly typed <see cref="Instance"/> is available) and then set <see cref="IsHandled"/> to
/// <c>true</c> to suppress the built-in value generation for that property.
/// </summary>
/// <typeparam name="T">The type of the object being populated.</typeparam>
public sealed class PropertyPopulation<T>
{
    internal PropertyPopulation(
        T instance,
        string propertyName,
        Type propertyType,
        bool randomize,
        int seedValue)
    {
        Instance = instance;
        PropertyName = propertyName;
        PropertyType = propertyType;
        Randomize = randomize;
        SeedValue = seedValue;
    }

    /// <summary>
    /// Gets the object being populated.
    /// </summary>
    public T Instance { get; }

    /// <summary>
    /// Gets the name of the property currently being populated.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the declared type of the property currently being populated.
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// Gets a value indicating whether the caller asked for randomized values. Honor this in custom
    /// logic so handled properties behave consistently with the generated ones.
    /// </summary>
    public bool Randomize { get; }

    /// <summary>
    /// Gets the seed value supplied by the caller (for example, an item's index in a collection). Use
    /// it to keep predictable custom values unique across a set of objects.
    /// </summary>
    public int SeedValue { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the caller has populated this property. When set to
    /// <c>true</c>, the built-in value generation is skipped and the property is reported as populated.
    /// </summary>
    public bool IsHandled { get; set; }
}
