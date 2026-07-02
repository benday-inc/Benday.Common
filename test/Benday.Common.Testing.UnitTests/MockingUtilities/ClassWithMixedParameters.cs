namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithMixedParameters
{
    public ClassWithMixedParameters(
        string name, ISampleRepository repository, int count)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        Name = name;
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        Count = count;
    }

    public string Name { get; private set; }
    public ISampleRepository Repository { get; private set; }
    public int Count { get; private set; }
}
