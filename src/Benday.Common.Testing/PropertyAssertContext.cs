using System;

namespace Benday.Common.Testing;

/// <summary>
/// Supplies the context for a single property while a reflection-based assertion (such as
/// <see cref="AssertThat.AllPropertiesAreNonNullAndNonDefaultValue{T}(T, Action{PropertyAssert{T}}, string[], string)"/>)
/// walks an object's properties. A test author can inspect the property and perform a custom
/// check, then set <see cref="IsHandled"/> to <c>true</c> to suppress the built-in assertion for
/// that property.
/// </summary>
/// <typeparam name="T">The type of the object being asserted.</typeparam>
public sealed class PropertyAssert<T>
{
    internal PropertyAssert(T instance, string propertyName, Type propertyType, object? value)
    {
        Instance = instance;
        PropertyName = propertyName;
        PropertyType = propertyType;
        Value = value;
    }

    /// <summary>
    /// Gets the object whose property is being asserted.
    /// </summary>
    public T Instance { get; }

    /// <summary>
    /// Gets the name of the property currently being asserted.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the declared type of the property currently being asserted.
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// Gets the current value of the property on <see cref="Instance"/>.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the test author has fully handled this property.
    /// When set to <c>true</c>, the built-in assertion is skipped for this property. A handler that
    /// sets this directly is expected to throw (for example via <see cref="AssertThat.Fail(string)"/>)
    /// when its own check fails. Prefer <see cref="HandleManuallyPass"/> /
    /// <see cref="HandleManuallyFail(string)"/>, which integrate the result into the aggregated
    /// failure report instead of failing on the first difference.
    /// </summary>
    public bool IsHandled { get; set; }

    internal bool HasManualFailure { get; private set; }

    internal string? ManualFailureMessage { get; private set; }

    /// <summary>
    /// Marks this property as fully handled and passing, suppressing the built-in check. Use this
    /// when your custom logic has confirmed the property is acceptable.
    /// </summary>
    public void HandleManuallyPass()
    {
        IsHandled = true;
        HasManualFailure = false;
        ManualFailureMessage = null;
    }

    /// <summary>
    /// Marks this property as fully handled and failing, suppressing the built-in check. The supplied
    /// <paramref name="failureMessage"/> is collected and reported alongside any other failures rather
    /// than thrown immediately, so a single assertion call surfaces every problem at once.
    /// </summary>
    /// <param name="failureMessage">The message describing why this property failed.</param>
    public void HandleManuallyFail(string failureMessage)
    {
        IsHandled = true;
        HasManualFailure = true;
        ManualFailureMessage = failureMessage;
    }
}

/// <summary>
/// Supplies the context for a single property while
/// <see cref="AssertThat.PropertiesAreEqual{T}(T, T, Action{PropertyComparison{T}}, string[], string)"/>
/// compares two objects. A test author can inspect both the expected and actual values, perform a
/// custom comparison, then set <see cref="IsHandled"/> to <c>true</c> to suppress the built-in
/// equality check for that property.
/// </summary>
/// <typeparam name="T">The type of the objects being compared.</typeparam>
public sealed class PropertyComparison<T>
{
    internal PropertyComparison(
        T expected,
        T actual,
        string propertyName,
        Type propertyType,
        object? expectedValue,
        object? actualValue)
    {
        Expected = expected;
        Actual = actual;
        PropertyName = propertyName;
        PropertyType = propertyType;
        ExpectedValue = expectedValue;
        ActualValue = actualValue;
    }

    /// <summary>
    /// Gets the expected object.
    /// </summary>
    public T Expected { get; }

    /// <summary>
    /// Gets the actual object.
    /// </summary>
    public T Actual { get; }

    /// <summary>
    /// Gets the name of the property currently being compared.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the declared type of the property currently being compared.
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// Gets the value of the property on <see cref="Expected"/>.
    /// </summary>
    public object? ExpectedValue { get; }

    /// <summary>
    /// Gets the value of the property on <see cref="Actual"/>.
    /// </summary>
    public object? ActualValue { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the test author has fully handled this property.
    /// When set to <c>true</c>, the built-in equality check is skipped for this property. A handler
    /// that sets this directly is expected to throw (for example via
    /// <see cref="AssertThat.Fail(string)"/>) when its own comparison fails. Prefer
    /// <see cref="HandleManuallyPass"/> / <see cref="HandleManuallyFail(string)"/>, which integrate
    /// the result into the aggregated failure report instead of failing on the first difference.
    /// </summary>
    public bool IsHandled { get; set; }

    internal bool HasManualFailure { get; private set; }

    internal string? ManualFailureMessage { get; private set; }

    /// <summary>
    /// Marks this property as fully handled and passing, suppressing the built-in equality check. Use
    /// this when your custom comparison has confirmed the values match.
    /// </summary>
    public void HandleManuallyPass()
    {
        IsHandled = true;
        HasManualFailure = false;
        ManualFailureMessage = null;
    }

    /// <summary>
    /// Marks this property as fully handled and failing, suppressing the built-in equality check. The
    /// supplied <paramref name="failureMessage"/> is collected and reported alongside any other
    /// mismatches rather than thrown immediately, so a single assertion call surfaces every difference
    /// at once.
    /// </summary>
    /// <param name="failureMessage">The message describing why this property failed.</param>
    public void HandleManuallyFail(string failureMessage)
    {
        IsHandled = true;
        HasManualFailure = true;
        ManualFailureMessage = failureMessage;
    }
}
