namespace Benday.Common.Testing;

public class CheckCollectionAssertion<T> : CheckAssertion<T>, ICheckCollectionAssertion<T>
{
    public CheckCollectionAssertion(T input) : base(input)
    {

    }
}

