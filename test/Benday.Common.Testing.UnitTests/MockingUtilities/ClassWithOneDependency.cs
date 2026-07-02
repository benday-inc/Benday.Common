namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithOneDependency
{
    public ClassWithOneDependency(ISampleRepository repository)
    {
        Repository = repository;
    }

    public ISampleRepository Repository { get; private set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

