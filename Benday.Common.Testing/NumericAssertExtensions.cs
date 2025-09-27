namespace Benday.Common.Testing;

/// <summary>
/// Provides fluent assertion extension methods for numeric values.
/// </summary>
public static class NumericAssertExtensions
{
    /// <summary>
    /// Verifies that a value is greater than the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (exclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the actual value is not greater than the minimum.</exception>
    public static T ShouldBeGreaterThan<T>(this T actual, T minimum, string message) where T : IComparable<T>
    {
        NumericAssert.IsGreaterThan(actual, minimum, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the actual value is not greater than or equal to the minimum.</exception>
    public static T ShouldBeGreaterThanOrEqualTo<T>(this T actual, T minimum, string message) where T : IComparable<T>
    {
        NumericAssert.IsGreaterThanOrEqualTo(actual, minimum, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is less than the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="maximum">The maximum value (exclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the actual value is not less than the maximum.</exception>
    public static T ShouldBeLessThan<T>(this T actual, T maximum, string message) where T : IComparable<T>
    {
        NumericAssert.IsLessThan(actual, maximum, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="maximum">The maximum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the actual value is not less than or equal to the maximum.</exception>
    public static T ShouldBeLessThanOrEqualTo<T>(this T actual, T maximum, string message) where T : IComparable<T>
    {
        NumericAssert.IsLessThanOrEqualTo(actual, maximum, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is within the specified range (inclusive).
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (inclusive).</param>
    /// <param name="maximum">The maximum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the actual value is not within the range.</exception>
    public static T ShouldBeInRange<T>(this T actual, T minimum, T maximum, string message) where T : IComparable<T>
    {
        NumericAssert.IsInRange(actual, minimum, maximum, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is not within the specified range (inclusive).
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (inclusive).</param>
    /// <param name="maximum">The maximum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the actual value is within the range.</exception>
    public static T ShouldNotBeInRange<T>(this T actual, T minimum, T maximum, string message) where T : IComparable<T>
    {
        NumericAssert.IsNotInRange(actual, minimum, maximum, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is not positive.</exception>
    public static T ShouldBePositive<T>(this T actual, string message) where T : IComparable<T>, new()
    {
        NumericAssert.IsPositive(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is negative (less than zero).
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is not negative.</exception>
    public static T ShouldBeNegative<T>(this T actual, string message) where T : IComparable<T>, new()
    {
        NumericAssert.IsNegative(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is zero.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is not zero.</exception>
    public static T ShouldBeZero<T>(this T actual, string message) where T : IComparable<T>, new()
    {
        NumericAssert.IsZero(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a value is not zero.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is zero.</exception>
    public static T ShouldNotBeZero<T>(this T actual, string message) where T : IComparable<T>, new()
    {
        NumericAssert.IsNotZero(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that two floating-point values are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the values are not approximately equal.</exception>
    public static double ShouldBeApproximatelyEqualTo(this double actual, double expected, double tolerance, string message)
    {
        NumericAssert.AreApproximatelyEqual(expected, actual, tolerance, message);
        return actual;
    }

    /// <summary>
    /// Verifies that two floating-point values are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the values are not approximately equal.</exception>
    public static float ShouldBeApproximatelyEqualTo(this float actual, float expected, float tolerance, string message)
    {
        NumericAssert.AreApproximatelyEqual(expected, actual, tolerance, message);
        return actual;
    }

    /// <summary>
    /// Verifies that two decimal values are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the values are not approximately equal.</exception>
    public static decimal ShouldBeApproximatelyEqualTo(this decimal actual, decimal expected, decimal tolerance, string message)
    {
        NumericAssert.AreApproximatelyEqual(expected, actual, tolerance, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a double value is not NaN (Not a Number).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is NaN.</exception>
    public static double ShouldNotBeNaN(this double actual, string message)
    {
        NumericAssert.IsNotNaN(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a float value is not NaN (Not a Number).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is NaN.</exception>
    public static float ShouldNotBeNaN(this float actual, string message)
    {
        NumericAssert.IsNotNaN(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a double value is finite (not infinity or NaN).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is not finite.</exception>
    public static double ShouldBeFinite(this double actual, string message)
    {
        NumericAssert.IsFinite(actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that a float value is finite (not infinity or NaN).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual value for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the value is not finite.</exception>
    public static float ShouldBeFinite(this float actual, string message)
    {
        NumericAssert.IsFinite(actual, message);
        return actual;
    }
}