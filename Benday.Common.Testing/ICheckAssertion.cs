namespace Benday.Common.Testing;

public interface ICheckCollectionAssertion<T> : ICheckAssertion<T>
{

}

public interface ICheckArrayAssertion<T> : 
    ICheckAssertion<T>
{

}

public interface ICheckAssertion<T>
{
    T? Input { get; }
    string? FailureMessage { get; }
    ICheckAssertion<T> WithMessage(string message);
}

