namespace Benday.Common.Testing;

public class CheckAssertion<T> : ICheckAssertion<T>
{
    public T? Input { get; private set; }

    public string? FailureMessage { get; set; }

    public CheckAssertion(T? input)
    {
        Input = input;
    }

    public ICheckAssertion<T> WithMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Failure message cannot be null or whitespace.", nameof(message));
        }
        FailureMessage = message;
        return this;
    }
}


