using System;
using System.Linq;

namespace PowerpointUtil.Api.Tests.MockingUtilities;

public class ClassWithEmptyConstructor
{
    public ClassWithEmptyConstructor()
    {

    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
