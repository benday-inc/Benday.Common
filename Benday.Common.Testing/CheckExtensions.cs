namespace Benday.Common.Testing;



public static class CheckExtensions
{
    //public static ICheckAssertion<T?> CheckThat<T?>(this T? input)
    //{
    //    return new CheckAssertion<T?>(input);
    //}

    //public static ICheckAssertion<T> CheckThat<T>(this T input) where T : notnull
    //{
    //    return new CheckAssertion<T>(input);
    //}

    public static ICheckAssertion<T> CheckThat<T>(this T input)
    {
        return new CheckAssertion<T>(input);
    }

    public static ICheckArrayAssertion<T[]> CheckThat<T>(this T[] input)
    {
        // specialized array handling
        return new CheckArrayAssertion<T[]>(input);
    }

    public static ICheckCollectionAssertion<T> CheckThatCollection<T>(this T input)
    {
        return new CheckCollectionAssertion<T>(input);
    }
}


