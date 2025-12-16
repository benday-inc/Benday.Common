using System;
using System.Diagnostics;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class ProcessRunnerFixture : TestClassBase
{
    public ProcessRunnerFixture(ITestOutputHelper output) : base(output)
    {
    }

    private ProcessRunner CreateSystemUnderTest(string fileName, string arguments = "")
    {
        var startInfo = new ProcessStartInfo(fileName, arguments);
        return new ProcessRunner(startInfo);
    }

    [Fact]
    public void Constructor_SetsStartInfo()
    {
        // arrange
        var startInfo = new ProcessStartInfo("echo", "hello");

        // act
        var sut = new ProcessRunner(startInfo);

        // assert
        sut.StartInfo.ShouldNotBeNull("StartInfo should not be null.");
        sut.StartInfo.FileName.ShouldEqual("echo", "FileName was wrong.");
    }

    [Fact]
    public void Constructor_ConfiguresStartInfoForOutputCapture()
    {
        // arrange
        var startInfo = new ProcessStartInfo("echo", "hello");

        // act
        var sut = new ProcessRunner(startInfo);

        // assert
        sut.StartInfo.RedirectStandardOutput.ShouldBeTrue("RedirectStandardOutput should be true.");
        sut.StartInfo.RedirectStandardError.ShouldBeTrue("RedirectStandardError should be true.");
        sut.StartInfo.UseShellExecute.ShouldBeFalse("UseShellExecute should be false.");
        sut.StartInfo.CreateNoWindow.ShouldBeTrue("CreateNoWindow should be true.");
    }

    [Fact]
    public void Timeout_DefaultValue()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo");
        var expectedTimeout = 10000;

        // act
        var actual = sut.Timeout;

        // assert
        actual.ShouldEqual(expectedTimeout, "Default timeout should be 10000ms.");
    }

    [Fact]
    public void Run_SuccessfulCommand_SetsIsSuccessTrue()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello world");

        // act
        var result = sut.Run();

        // assert
        sut.IsSuccess.ShouldBeTrue("IsSuccess should be true for successful command.");
        sut.IsError.ShouldBeFalse("IsError should be false for successful command.");
        sut.HasCompleted.ShouldBeTrue("HasCompleted should be true.");
        result.ShouldNotBeNull("Result should not be null.");
        result.IsSuccess.ShouldBeTrue("Result.IsSuccess should be true.");
    }

    [Fact]
    public void Run_SuccessfulCommand_CapturesOutput()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello world");

        // act
        var result = sut.Run();

        // assert
        sut.OutputText.ShouldContain("hello world", "OutputText should contain the echoed text.");
        result.OutputText.ShouldContain("hello world", "Result.OutputText should contain the echoed text.");
    }

    [Fact]
    public void Run_SuccessfulCommand_ReturnsExitCodeZero()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");

        // act
        var result = sut.Run();

        // assert
        sut.ExitCode.ShouldEqual(0, "ExitCode should be 0 for successful command.");
        result.ExitCode.ShouldEqual(0, "Result.ExitCode should be 0.");
    }

    [Fact]
    public void Run_FailingCommand_SetsIsErrorTrue()
    {
        // arrange - use a command that will fail
        var sut = CreateSystemUnderTest("ls", "/nonexistent/directory/that/does/not/exist");

        // act
        var result = sut.Run();

        // assert
        sut.IsError.ShouldBeTrue("IsError should be true for failing command.");
        sut.IsSuccess.ShouldBeFalse("IsSuccess should be false for failing command.");
        sut.HasCompleted.ShouldBeTrue("HasCompleted should be true.");
        result.IsError.ShouldBeTrue("Result.IsError should be true.");
    }

    [Fact]
    public void Run_FailingCommand_ReturnsNonZeroExitCode()
    {
        // arrange
        var sut = CreateSystemUnderTest("ls", "/nonexistent/directory/that/does/not/exist");

        // act
        var result = sut.Run();

        // assert
        sut.ExitCode.ShouldNotEqual(0, "ExitCode should not be 0 for failing command.");
        result.ExitCode.ShouldNotEqual(0, "Result.ExitCode should not be 0.");
    }

    [Fact]
    public void Run_CalledTwice_ThrowsInvalidOperationException()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");
        sut.Run();

        // act & assert
        Assert.Throws<InvalidOperationException>(() => sut.Run());
    }

    [Fact]
    public void Run_WithTimeout_ThrowsTimeoutException()
    {
        // arrange - use sleep command with short timeout
        var sut = CreateSystemUnderTest("sleep", "10");
        sut.Timeout = 100; // 100ms timeout

        // act & assert
        Assert.Throws<TimeoutException>(() => sut.Run());
        sut.IsTimeout.ShouldBeTrue("IsTimeout should be true after timeout.");
    }

    [Fact]
    public void ImplementsIProcessRunner()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");

        // act & assert
        Assert.IsAssignableFrom<IProcessRunner>(sut);
    }

    [Fact]
    public void Run_ReturnsIProcessRunnerResult()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");

        // act
        var result = sut.Run();

        // assert
        Assert.IsAssignableFrom<IProcessRunnerResult>(result);
    }
}
