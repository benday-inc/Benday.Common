using System;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class ClassForTesting
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
