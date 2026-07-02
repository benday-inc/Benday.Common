namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithTwoStringsAndInterface
{
    public ClassWithTwoStringsAndInterface(
        string firstName, ISampleRepository repository, string lastName, int age)
    {
        FirstName = firstName;
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        LastName = lastName;
        Age = age;
    }

    public string FirstName { get; private set; }
    public ISampleRepository Repository { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }
}
