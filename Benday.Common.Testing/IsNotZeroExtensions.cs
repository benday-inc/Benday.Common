namespace Benday.Common.Testing;

public static class IsNotZeroExtensions
{
    public static ICheckAssertion<T> IsNotZero<T>(
        this ICheckAssertion<T> check,
        string? userFailureMessage = null)
    {
        if (check.Input is null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (typeof(T) == typeof(int))
        {
            var input = check.Input as int?;

            if (input == 0)
            {
                check.FailWithOptionalMessage(
                    userFailureMessage,
                    $"Value should be equal to zero. Actual value was '{input}'");
            }
        }
        else if (typeof(T) == typeof(float))
        {
            var input = check.Input as float?;

            if (input == 0)
            {
                check.FailWithOptionalMessage(
                    userFailureMessage,
                    $"Value should be equal to zero. Actual value was '{input}'");
            }
        }
        else
        {
            check.FailWithOptionalMessage(
                $"Method IsNotZero is not implemented for type {typeof(T).Name}.");
        }

        return check;
    }
}
