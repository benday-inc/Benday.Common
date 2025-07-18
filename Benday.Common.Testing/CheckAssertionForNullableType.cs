namespace Benday.Common.Testing;

public class CheckAssertionForNullableType<T> : CheckAssertion<T>, ICheckAssertionForNullableType<T>
{
    public CheckAssertionForNullableType(T input) : base(input)
    {
    }
}



