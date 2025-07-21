namespace Benday.Common.Testing;



public static class CheckExtensions
{
    public static ICheckAssertion<string> CheckThat(this string input)
    {
        return new CheckAssertion<string>(input);
    }

    public static ICheckAssertion<T> CheckThat<T>(this T input) where T : notnull
    {
        var type = typeof(T);

        if (type.IsArray == true)
        {
            throw new WrongCheckThatMethodException(
                $"Cannot start asserting using this method for type {type}. Call {nameof(CheckThatArray)} instead.");
        }
        else
        {
            // check if T is a collection type
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) &&
                type != typeof(string))
            {
                throw new WrongCheckThatMethodException(
                    $"Cannot start asserting using this method for type {type}. Call {nameof(CheckThatCollection)} instead.");
            }
        }

        return new CheckAssertion<T>(input);
    }   

    public static ICheckAssertionForNullableType<T?> CheckThatNullable<T>(this T? input)
    {
        return new CheckAssertionForNullableType<T?>(input);
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

    public static ICheckArrayAssertion<T[]> CheckThatArray<T>(this T[] input)
    {
        return new CheckArrayAssertion<T[]>(input);
    }

    public static ICheckCollectionAssertion<T> CheckThatCollection<T>(this T input)
    {
        return new CheckCollectionAssertion<T>(input);
    }
}



