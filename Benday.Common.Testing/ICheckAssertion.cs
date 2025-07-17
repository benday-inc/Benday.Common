namespace Benday.Common.Testing;

public interface ICheckAssertion<T>
{
    T? Input { get; }
    string? FailureMessage { get; }
    ICheckAssertion<T> WithMessage(string message);
}

