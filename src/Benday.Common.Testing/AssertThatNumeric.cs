namespace Benday.Common.Testing;

/// <summary>
/// Provides static methods for making assertions on numeric values with descriptive failure messages.
/// </summary>
public static class AssertThatNumeric
{
    /// <summary>
    /// Verifies that a value is greater than the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (exclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the actual value is not greater than the minimum.</exception>
    public static void IsGreaterThan<T>(T actual, T minimum, string message) where T : IComparable<T>
    {
        if (actual.CompareTo(minimum) <= 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsGreaterThan",
                $"Expected value greater than: {AssertionMessageFormatter.FormatValue(minimum)}\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the actual value is not greater than or equal to the minimum.</exception>
    public static void IsGreaterThanOrEqualTo<T>(T actual, T minimum, string message) where T : IComparable<T>
    {
        if (actual.CompareTo(minimum) < 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsGreaterThanOrEqualTo",
                $"Expected value greater than or equal to: {AssertionMessageFormatter.FormatValue(minimum)}\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is less than the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="maximum">The maximum value (exclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the actual value is not less than the maximum.</exception>
    public static void IsLessThan<T>(T actual, T maximum, string message) where T : IComparable<T>
    {
        if (actual.CompareTo(maximum) >= 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsLessThan",
                $"Expected value less than: {AssertionMessageFormatter.FormatValue(maximum)}\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="maximum">The maximum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the actual value is not less than or equal to the maximum.</exception>
    public static void IsLessThanOrEqualTo<T>(T actual, T maximum, string message) where T : IComparable<T>
    {
        if (actual.CompareTo(maximum) > 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsLessThanOrEqualTo",
                $"Expected value less than or equal to: {AssertionMessageFormatter.FormatValue(maximum)}\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is within the specified range (inclusive).
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (inclusive).</param>
    /// <param name="maximum">The maximum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the actual value is not within the range.</exception>
    public static void IsInRange<T>(T actual, T minimum, T maximum, string message) where T : IComparable<T>
    {
        if (actual.CompareTo(minimum) < 0 || actual.CompareTo(maximum) > 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsInRange",
                $"Expected value in range [{AssertionMessageFormatter.FormatValue(minimum)}, {AssertionMessageFormatter.FormatValue(maximum)}]\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is not within the specified range (inclusive).
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="minimum">The minimum value (inclusive).</param>
    /// <param name="maximum">The maximum value (inclusive).</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the actual value is within the range.</exception>
    public static void IsNotInRange<T>(T actual, T minimum, T maximum, string message) where T : IComparable<T>
    {
        if (actual.CompareTo(minimum) >= 0 && actual.CompareTo(maximum) <= 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsNotInRange",
                $"Expected value not in range [{AssertionMessageFormatter.FormatValue(minimum)}, {AssertionMessageFormatter.FormatValue(maximum)}]\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not positive.</exception>
    public static void IsPositive<T>(T actual, string message) where T : IComparable<T>, new()
    {
        var zero = new T();
        if (actual.CompareTo(zero) <= 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsPositive",
                $"Expected positive value\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is negative (less than zero).
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not negative.</exception>
    public static void IsNegative<T>(T actual, string message) where T : IComparable<T>, new()
    {
        var zero = new T();
        if (actual.CompareTo(zero) >= 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsNegative",
                $"Expected negative value\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is zero.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not zero.</exception>
    public static void IsZero<T>(T actual, string message) where T : IComparable<T>, new()
    {
        var zero = new T();
        if (actual.CompareTo(zero) != 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsZero",
                $"Expected zero\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a value is not zero.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is zero.</exception>
    public static void IsNotZero<T>(T actual, string message) where T : IComparable<T>, new()
    {
        var zero = new T();
        if (actual.CompareTo(zero) == 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsNotZero",
                $"Expected non-zero value\nActual value: {AssertionMessageFormatter.FormatValue(actual)}"));
        }
    }

    /// <summary>
    /// Verifies that two floating-point values are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the values are not approximately equal.</exception>
    public static void AreApproximatelyEqual(double expected, double actual, double tolerance, string message)
    {
        if (Math.Abs(expected - actual) > tolerance)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "AreApproximatelyEqual",
                $"Expected: {expected} ± {tolerance}\nActual: {actual}\nDifference: {Math.Abs(expected - actual)}"));
        }
    }

    /// <summary>
    /// Verifies that two floating-point values are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the values are not approximately equal.</exception>
    public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string message)
    {
        if (Math.Abs(expected - actual) > tolerance)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "AreApproximatelyEqual",
                $"Expected: {expected} ± {tolerance}\nActual: {actual}\nDifference: {Math.Abs(expected - actual)}"));
        }
    }

    /// <summary>
    /// Verifies that two decimal values are approximately equal within the specified tolerance.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="tolerance">The tolerance for comparison.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the values are not approximately equal.</exception>
    public static void AreApproximatelyEqual(decimal expected, decimal actual, decimal tolerance, string message)
    {
        if (Math.Abs(expected - actual) > tolerance)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "AreApproximatelyEqual",
                $"Expected: {expected} ± {tolerance}\nActual: {actual}\nDifference: {Math.Abs(expected - actual)}"));
        }
    }

    /// <summary>
    /// Verifies that a double value is not NaN (Not a Number).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is NaN.</exception>
    public static void IsNotNaN(double actual, string message)
    {
        if (double.IsNaN(actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsNotNaN",
                "Expected value not to be NaN"));
        }
    }

    /// <summary>
    /// Verifies that a float value is not NaN (Not a Number).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is NaN.</exception>
    public static void IsNotNaN(float actual, string message)
    {
        if (float.IsNaN(actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsNotNaN",
                "Expected value not to be NaN"));
        }
    }

    /// <summary>
    /// Verifies that a double value is finite (not infinity or NaN).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not finite.</exception>
    public static void IsFinite(double actual, string message)
    {
        if (!double.IsFinite(actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsFinite",
                $"Expected finite value\nActual value: {actual}"));
        }
    }

    /// <summary>
    /// Verifies that a float value is finite (not infinity or NaN).
    /// </summary>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the value is not finite.</exception>
    public static void IsFinite(float actual, string message)
    {
        if (!float.IsFinite(actual))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatSimpleMessage(message, "IsFinite",
                $"Expected finite value\nActual value: {actual}"));
        }
    }
}