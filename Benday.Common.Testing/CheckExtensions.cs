namespace Benday.Common.Testing;



public static class CheckExtensions
{
    public static ICheckAssertion<T> CheckThat<T>(this T input) where T : notnull
    {
        return new CheckAssertion<T>(input);
    }

    public static ICheckAssertion<string> CheckThat(this string input)
    {
        return new CheckAssertion<string>(input);
    }

    public static ICheckAssertionForNullableType<string?> CheckThatNullable(
        this string? input)
    {
        return new CheckAssertionForNullableType<string?>(input);
    }

    public static ICheckAssertionForNullableType<string?[]> CheckThatNullable(
       this string?[] input)
    {
        return new CheckAssertionForNullableType<string?[]>(input);
    }

    public static ICheckArrayAssertion<T[]> CheckThat<T>(this T[] input)
    {
        return new CheckArrayAssertion<T[]>(input);
    }

    public static ICheckCollectionAssertion<T> CheckThatCollection<T>(this T input)
    {
        return new CheckCollectionAssertion<T>(input);
    }
}


