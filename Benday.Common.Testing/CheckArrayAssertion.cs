namespace Benday.Common.Testing;


public class CheckArrayAssertion<T> : CheckAssertion<T>, ICheckArrayAssertion<T>
{
    public CheckArrayAssertion(T input) : base(input)
    {
    }
}



