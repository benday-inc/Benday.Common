namespace Benday.Common.Testing;



public static class CheckExtensions
{
    public static ICheckAssertion<T> CheckThat<T>(this T? input)
    {
        return new CheckAssertion<T>(input);
    }
}


