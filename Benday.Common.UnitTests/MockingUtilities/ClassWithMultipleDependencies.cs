using Microsoft.Extensions.Logging;
using PowerpointUtil.Api.Repositories;
using System;
using System.Linq;

namespace PowerpointUtil.Api.Tests.MockingUtilities;

public class ClassWithMultipleDependencies
{
    public ClassWithMultipleDependencies(
        ISlideDataRepository repository, ILogger<ClassWithMultipleDependencies> logger)
    {
        Repository = repository;
        Logger = logger;
    }

    public ISlideDataRepository Repository { get; private set; }
    public ILogger<ClassWithMultipleDependencies> Logger { get; private set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
