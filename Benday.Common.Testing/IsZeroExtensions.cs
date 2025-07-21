namespace Benday.Common.Testing;

public static class IsZeroExtensions
{
    public static ICheckAssertion<T> IsZero<T>(this ICheckAssertion<T> check,
        string? userFailureMessage = null)
    {
        if (check.Input is null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (typeof(T) == typeof(int))
        {
            var input = check.Input as int?;

            if (input != 0)
            {
                check.FailWithOptionalMessage(
                    userFailureMessage,
                    $"Values should be equal to zero. Expected '0' but actual value was '{input}'");
            }
        }
        else if (typeof(T) == typeof(float))
        {
            var input = check.Input as float?;

            if (input != 0)
            {
                check.FailWithOptionalMessage(
                    userFailureMessage,
                    $"Value should be equal to zero. Expected '0' but actual value was '{input}'");
            }
        }
        else
        {
            check.FailWithOptionalMessage(
                $"Method IsZero is not implemented for type {typeof(T).Name}.");
        }

        return check;
    }
}

public static class ContainsExtensions
{
    public static ICheckAssertion<T> Contains<T>(this ICheckAssertion<T> check,
        T expected,
        string? userFailureMessage = null)
    {
        if (check.Input is null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (typeof(T) == typeof(string))
        {
            var input = check.Input as string;
            var expectedAsString = expected as string;

            if (input is null)
            {
                check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
            }
            else if (expectedAsString is null)
                {
                check.FailWithOptionalMessage(userFailureMessage, "Expected value is null.");
            }
            else if (input.Contains(expectedAsString) == false)
            {
                check.FailWithOptionalMessage(
                    userFailureMessage,
                    $"Input does not contain expected value. Expected '{expectedAsString}' but actual value was '{input}'");
            }
        }
        else if (typeof(T).IsArray == true)
        {
            // does the array contain the expected value?
            var inputArray = check.Input as Array;

            var expectedArray = expected as Array;
        }
        else
        {
            check.FailWithOptionalMessage(
                $"Method IsZero is not implemented for type {typeof(T).Name}.");
        }

        return check;
    }
}