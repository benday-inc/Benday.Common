namespace Benday.Common.Testing;

public static class IsNotEquivalentToExtensions
{
    public static ICheckAssertion<T> IsNotEquivalentTo<T>(this ICheckAssertion<T> check,
        T expected,
        string? userFailureMessage = null)
    {
        if (check.Input is null || check.Input.Equals(expected) == true)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should not be equivalent. Expected anything but '{expected}' but actual value was '{check.Input}'");
        }
        return check;
    }
}