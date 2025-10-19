using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Benday.Common.Testing;

/// <summary>
/// Provides static methods for making assertions in tests with descriptive failure messages.
/// </summary>
public static class AssertThat
{
    /// <summary>
    /// Verifies that the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to verify.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the condition is false.</exception>
    public static void IsTrue([DoesNotReturnIf(false)] bool condition, string message)
    {
        if (!condition)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsTrue", "Expected: True"));
        }
    }

    /// <summary>
    /// Verifies that the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition to verify.</param>
    /// <exception cref="AssertionException">Thrown when the condition is false.</exception>
    public static void IsTrue([DoesNotReturnIf(false)] bool condition)
    {
        if (!condition)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "IsTrue", "Expected: True"));
        }
    }

    /// <summary>
    /// Verifies that the specified condition is false.
    /// </summary>
    /// <param name="condition">The condition to verify.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the condition is true.</exception>
    public static void IsFalse([DoesNotReturnIf(true)] bool condition, string message)
    {
        if (condition)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsFalse", "Expected: False"));
        }
    }

    /// <summary>
    /// Verifies that the specified condition is false.
    /// </summary>
    /// <param name="condition">The condition to verify.</param>
    /// <exception cref="AssertionException">Thrown when the condition is true.</exception>
    public static void IsFalse([DoesNotReturnIf(true)] bool condition)
    {
        if (condition)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "IsFalse", "Expected: False"));
        }
    }

    /// <summary>
    /// Verifies that two values are equal.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the values are not equal.</exception>
    public static void AreEqual<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEqual"));
        }
    }

    /// <summary>
    /// Verifies that two values are equal.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertionException">Thrown when the values are not equal.</exception>
    public static void AreEqual<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(
                expected, actual, string.Empty, "AreEqual"));
        }
    }

    /// <summary>
    /// Verifies that two values are not equal.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="expected">The value that should not match the actual value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the values are equal.</exception>
    public static void AreNotEqual<T>(T expected, T actual, string message)
    {
        if (EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreNotEqual"));
        }
    }

    /// <summary>
    /// Verifies that two values are not equal.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="expected">The value that should not match the actual value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertionException">Thrown when the values are equal.</exception>
    public static void AreNotEqual<T>(T expected, T actual)
    {
        if (EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, "AreNotEqual"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not null.</exception>
    public static void IsNull<T>(T? value, string message) where T : class
    {
        if (value != null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(null, value, message, "IsNull"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <exception cref="AssertionException">Thrown when the value is not null.</exception>
    public static void IsNull<T>(T? value) where T : class
    {
        if (value != null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(null, value, string.Empty, "IsNull"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is null.</exception>
    public static void IsNotNull<T>([NotNull] T? value, string message) where T : class
    {
        if (value == null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage("<not null>", null, message, "IsNotNull"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <exception cref="AssertionException">Thrown when the value is null.</exception>
    public static void IsNotNull<T>([NotNull] T? value) where T : class
    {
        if (value == null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage("<not null>", null, string.Empty, "IsNotNull"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is of the expected type.
    /// </summary>
    /// <typeparam name="T">The expected type.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not of the expected type.</exception>
    public static void IsOfType<T>(object? value, string message)
    {
        if (value == null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsOfType", $"Expected type: {typeof(T).Name}, but value was null"));
        }

        if (!(value is T))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsOfType",
                $"Expected type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, Actual type: {AssertionMessageFormatter.GetTypeName(value.GetType())}"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is of the expected type.
    /// </summary>
    /// <typeparam name="T">The expected type.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <exception cref="AssertionException">Thrown when the value is not of the expected type.</exception>
    public static void IsOfType<T>(object? value)
    {
        if (value == null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "IsOfType", $"Expected type: {typeof(T).Name}, but value was null"));
        }

        if (!(value is T))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "IsOfType",
                $"Expected type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, Actual type: {AssertionMessageFormatter.GetTypeName(value.GetType())}"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is not of the specified type.
    /// </summary>
    /// <typeparam name="T">The type that the value should not be.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is of the specified type.</exception>
    public static void IsNotOfType<T>(object? value, string message)
    {
        if (value is T)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsNotOfType",
                $"Value should not be of type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, but it was"));
        }
    }

    /// <summary>
    /// Verifies that the specified value is not of the specified type.
    /// </summary>
    /// <typeparam name="T">The type that the value should not be.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <exception cref="AssertionException">Thrown when the value is of the specified type.</exception>
    public static void IsNotOfType<T>(object? value)
    {
        if (value is T)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "IsNotOfType",
                $"Value should not be of type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, but it was"));
        }
    }

    /// <summary>
    /// Verifies that the specified action throws an exception of the expected type.
    /// </summary>
    /// <typeparam name="T">The expected exception type.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The thrown exception.</returns>
    /// <exception cref="AssertionException">Thrown when no exception or wrong exception type is thrown.</exception>
    public static T Throws<T>(Action action, string message) where T : Exception
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        try
        {
            action();
        }
        catch (T ex)
        {
            return ex;
        }
        catch (Exception ex)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "Throws",
                $"Expected exception type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, Actual exception type: {AssertionMessageFormatter.GetTypeName(ex.GetType())}"));
        }

        throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "Throws",
            $"Expected exception type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, but no exception was thrown"));
    }

    /// <summary>
    /// Verifies that the specified action throws an exception of the expected type.
    /// </summary>
    /// <typeparam name="T">The expected exception type.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <returns>The thrown exception.</returns>
    /// <exception cref="AssertionException">Thrown when no exception or wrong exception type is thrown.</exception>
    public static T Throws<T>(Action action) where T : Exception
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        try
        {
            action();
        }
        catch (T ex)
        {
            return ex;
        }
        catch (Exception ex)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "Throws",
                $"Expected exception type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, Actual exception type: {AssertionMessageFormatter.GetTypeName(ex.GetType())}"));
        }

        throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "Throws",
            $"Expected exception type: {AssertionMessageFormatter.GetTypeName(typeof(T))}, but no exception was thrown"));
    }

    /// <summary>
    /// Verifies that the specified action does not throw any exception.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when an exception is thrown.</exception>
    public static void DoesNotThrow(Action action, string message)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        try
        {
            action();
        }
        catch (Exception ex)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "DoesNotThrow",
                $"Expected no exception, but {AssertionMessageFormatter.GetTypeName(ex.GetType())} was thrown: {ex.Message}"));
        }
    }

    /// <summary>
    /// Verifies that the specified action does not throw any exception.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <exception cref="AssertionException">Thrown when an exception is thrown.</exception>
    public static void DoesNotThrow(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        try
        {
            action();
        }
        catch (Exception ex)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(string.Empty, "DoesNotThrow",
                $"Expected no exception, but {AssertionMessageFormatter.GetTypeName(ex.GetType())} was thrown: {ex.Message}"));
        }
    }

    /// <summary>
    /// Verifies that the two object references refer to the same object.
    /// </summary>
    /// <param name="expected">The expected object reference.</param>
    /// <param name="actual">The actual object reference.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the references are not the same.</exception>
    public static void AreSame(object? expected, object? actual, string message)
    {
        if (!ReferenceEquals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreSame"));
        }
    }

    /// <summary>
    /// Verifies that the two object references refer to the same object.
    /// </summary>
    /// <param name="expected">The expected object reference.</param>
    /// <param name="actual">The actual object reference.</param>
    /// <exception cref="AssertionException">Thrown when the references are not the same.</exception>
    public static void AreSame(object? expected, object? actual)
    {
        if (!ReferenceEquals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, "AreSame"));
        }
    }

    /// <summary>
    /// Verifies that the two object references do not refer to the same object.
    /// </summary>
    /// <param name="expected">The object reference that should not match the actual reference.</param>
    /// <param name="actual">The actual object reference.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the references are the same.</exception>
    public static void AreNotSame(object? expected, object? actual, string message)
    {
        if (ReferenceEquals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreNotSame"));
        }
    }

    /// <summary>
    /// Verifies that the two object references do not refer to the same object.
    /// </summary>
    /// <param name="expected">The object reference that should not match the actual reference.</param>
    /// <param name="actual">The actual object reference.</param>
    /// <exception cref="AssertionException">Thrown when the references are the same.</exception>
    public static void AreNotSame(object? expected, object? actual)
    {
        if (ReferenceEquals(expected, actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, string.Empty, "AreNotSame"));
        }
    }

    /// <summary>
    /// Fails the test with the specified message.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <exception cref="AssertionException">Always thrown.</exception>
    [DoesNotReturn]
    public static void Fail(string message)
    {
        throw new AssertionException(message);
    }
}