using System;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForStringsFixture : TestClassBase
{
    public CheckThatForStringsFixture(ITestOutputHelper output) : base(output)
    {
        
    }

    private string? ValueToTest { get; set; } = null;

    private ICheckAssertion<string>? _SystemUnderTest;
    public ICheckAssertion<string> SystemUnderTest
    {
        get
        {
            if (_SystemUnderTest == null)
            {
                _SystemUnderTest = ValueToTest.CheckThat();
            }

            if (_SystemUnderTest == null)
            {
                Assert.Fail("SystemUnderTest was still null after initialization.");
            }

            return _SystemUnderTest;
        }
    }

    [Fact]
    public void CheckThat_Null_IsNotNullOrEmpty()
    {
        ValueToTest = null;
        Assert.Throws<CheckAssertionFailureException>(() => SystemUnderTest.IsNotNullOrEmpty());
    }

    [Fact]
    public void CheckThat_StringIsNotNullOrEmpty()
    {
        ValueToTest = "Hello, World!";
        SystemUnderTest.IsNotNullOrEmpty();
    }

    [Fact]
    public void CheckThat_StringIsNullOrEmpty_ThrowsArgumentException()
    {
        string? input = "";
        Assert.Throws<CheckAssertionFailureException>(() => input.CheckThat().IsNotNullOrEmpty());
    }
}