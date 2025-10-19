using System;

using Xunit.Abstractions;

namespace Benday.Common.Testing.UnitTests.Assertions;



public class AssertThatStringFixture : TestClassBase
{
    public AssertThatStringFixture(ITestOutputHelper output) : base(output)
    {

    }

    [Fact]
    public void AreEqual_WithoutMessage_ShouldThrowWithDescriptiveErrorMessage()
    {
        // arrange
        string value1 = "value1";
        string value2 = "value2";

        // act
        var exception = Assert.Throws<AssertionException>(() =>
            AssertThat.AreEqual(value2, value1));

        // assert
        var actualMessage = exception.Message;

        WriteLine($"Actual message: {actualMessage}");

        Assert.Contains($"'{value1}'", actualMessage);
        Assert.Contains($"'{value2}'", actualMessage);
    }

    [Fact]
    public void AreEqual_WithMessage_ShouldThrowWithDescriptiveErrorMessage()
    {
        // arrange
        string value1 = "value1";
        string value2 = "value2";
        string message = "this is a message";

        // act
        var exception = Assert.Throws<AssertionException>(() =>
            AssertThat.AreEqual(value2, value1, message));

        // assert
        var actualMessage = exception.Message;

        WriteLine($"Actual message: {actualMessage}");

        Assert.Contains($"'{value1}'", actualMessage);
        Assert.Contains($"'{value2}'", actualMessage);
        Assert.Contains(message, actualMessage);
    }

}