using System.Diagnostics.CodeAnalysis;

namespace Benday.Common.Testing;

/// <summary>
/// Provides fluent assertion extension methods for all objects.
/// </summary>
public static class ObjectAssertExtensions
{
    /// <summary>
    /// Verifies that the object is equal to the expected value.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the values are not equal.</exception>
    public static T ShouldEqual<T>(this T actual, T expected, string message)
    {
        Assert.AreEqual(expected, actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the object is not equal to the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="notExpected">The value that should not match.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the values are equal.</exception>
    public static T ShouldNotEqual<T>(this T actual, T notExpected, string message)
    {
        Assert.AreNotEqual(notExpected, actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the object is null.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the object is not null.</exception>
    public static T? ShouldBeNull<T>([NotNull] this T? actual, string message) where T : class
    {
        Assert.IsNull(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the object is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the object is null.</exception>
    public static T ShouldNotBeNull<T>([NotNull] this T? actual, string message) where T : class
    {
        Assert.IsNotNull(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the object is of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected type.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object cast to the expected type for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the object is not of the expected type.</exception>
    public static T ShouldBeOfType<T>(this object? actual, string message)
    {
        Assert.IsOfType<T>(actual, message);
        return (T)actual!;
    }

    /// <summary>
    /// Verifies that the object is not of the specified type.
    /// </summary>
    /// <typeparam name="T">The type that the object should not be.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the object is of the specified type.</exception>
    public static object? ShouldNotBeOfType<T>(this object? actual, string message)
    {
        Assert.IsNotOfType<T>(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the two object references refer to the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="expected">The expected object reference.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the references are not the same.</exception>
    public static T ShouldBeSameAs<T>(this T actual, T expected, string message) where T : class
    {
        Assert.AreSame(expected, actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the two object references do not refer to the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="actual">The actual object.</param>
    /// <param name="notExpected">The object reference that should not match.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual object for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the references are the same.</exception>
    public static T ShouldNotBeSameAs<T>(this T actual, T notExpected, string message) where T : class
    {
        Assert.AreNotSame(notExpected, actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the action throws an exception of the specified type.
    /// </summary>
    /// <typeparam name="T">The expected exception type.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The thrown exception for further verification.</returns>
    /// <exception cref="AssertionException">Thrown when no exception or wrong exception type is thrown.</exception>
    public static T ShouldThrow<T>(this Action action, string message) where T : Exception
    {
        return Assert.Throws<T>(action, message);
    }

    /// <summary>
    /// Verifies that the action does not throw any exception.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The action for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when an exception is thrown.</exception>
    public static Action ShouldNotThrow(this Action action, string message)
    {
        Assert.DoesNotThrow(action, message);
        return action;
    }

    /// <summary>
    /// Verifies that the boolean value is true.
    /// </summary>
    /// <param name="actual">The actual boolean value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is false.</exception>
    public static bool ShouldBeTrue([DoesNotReturnIf(false)] this bool actual, string message)
    {
        Assert.IsTrue(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the boolean value is false.
    /// </summary>
    /// <param name="actual">The actual boolean value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is true.</exception>
    public static bool ShouldBeFalse([DoesNotReturnIf(true)] this bool actual, string message)
    {
        Assert.IsFalse(actual, message);
        return actual;
    }
}