﻿using Xunit.Abstractions;

namespace Benday.Common.Testing;

public class TestClassBase
{
    private readonly ITestOutputHelper _output;

    public TestClassBase(ITestOutputHelper output)
    {
        _output = output;
    }

    public void WriteLine(string message)
    {
        _output.WriteLine(message);
    }
}
