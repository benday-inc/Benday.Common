using Microsoft.Extensions.Logging;

namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithMultipleDependencies
{
    public ClassWithMultipleDependencies(
        ISampleRepository repository, ILogger<ClassWithMultipleDependencies> logger)
    {
        Repository = repository;
        Logger = logger;
    }

    public ISampleRepository Repository { get; private set; }
    public ILogger<ClassWithMultipleDependencies> Logger { get; private set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
