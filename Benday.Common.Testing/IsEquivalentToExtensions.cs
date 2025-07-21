namespace Benday.Common.Testing;

public static class IsEquivalentToExtensions
{
    public static ICheckAssertion<T> IsEquivalentTo<T>(this ICheckAssertion<T> check,
        T expected,
        string? userFailureMessage = null)
    {
        if (check.Input is null || check.Input.Equals(expected) == false)
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Values should be equivalent. Expected '{expected}' but actual value was '{check.Input}'");
        }
        return check;
    }
}
