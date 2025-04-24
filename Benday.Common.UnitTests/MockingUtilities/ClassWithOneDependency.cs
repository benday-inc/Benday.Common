using Castle.Core.Logging;
using PowerpointUtil.Api.Repositories;
using System;
using System.Linq;

namespace PowerpointUtil.Api.Tests.MockingUtilities;

public class ClassWithOneDependency
{
    public ClassWithOneDependency(ISlideDataRepository repository)
    {
        Repository = repository;
    }

    public ISlideDataRepository Repository { get; private set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
