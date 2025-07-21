
using Benday.Common.Testing;

using System;

using Xunit.Abstractions;

namespace Benday.Common.UnitTests.CheckThatAssertions;

public class CheckThatForInt32Fixture : TestClassBase
{
    public CheckThatForInt32Fixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void CheckThat_Nullable_IsNotNull_WhenValueIsNull()
    {
        int? input = null;

        var check = input.CheckThatNullable();

        Assert.Throws<CheckAssertionFailureException>(() => check.IsNotNull());
    }

    [Fact]
    public void CheckThat_Nullable_IsNotNull_WhenValueIsNotNull()
    {
        int? input = 1;

        var check = input.CheckThatNullable();

        check.IsNotNull();
    }

    [Fact]
    public void CheckThat_Nullable_IsNotZero_WhenValueIsNotZero()
    {
        int? input = 1;

        var check = input.CheckThatNullable();

        check.IsNotZero();
    }

    [Fact]
    public void CheckThat_Nullable_IsNotZero_WhenValueIsZero()
    {
        int? input = 0;

        var check = input.CheckThatNullable();

        Assert.Throws<CheckAssertionFailureException>(() => check.IsNotZero());
    }

    [Fact]
    public void CheckThat_Nullable_IsZero_WhenValueIsNotZero()
    {
        int? input = 1;

        var check = input.CheckThatNullable();

        Assert.Throws<CheckAssertionFailureException>(() => check.IsZero());
    }

    [Fact]
    public void CheckThat_Nullable_IsZero_WhenValueIsZero()
    {
        int? input = 0;

        var check = input.CheckThatNullable();

        check.IsZero();
    }

    [Fact]
    public void CheckThat_Nullable_IsZero_WhenValueIsNull()
    {
        int? input = null;

        var check = input.CheckThatNullable();

        Assert.Throws<CheckAssertionFailureException>(() => check.IsZero());
    }
}