namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithMultipleConstructors
{
    public ClassWithMultipleConstructors(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        Name = name;
        Repository = null;
    }

    public ClassWithMultipleConstructors(string name, ISampleRepository repository)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        Name = name;
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public string Name { get; private set; }
    public ISampleRepository? Repository { get; private set; }
}
