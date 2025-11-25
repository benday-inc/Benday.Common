using System;
using System.Diagnostics;

using Benday.Common.Testing;

using FluentAssertions;

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
        sut.StartInfo.Should().NotBeNull("StartInfo should not be null.");
        sut.StartInfo.FileName.Should().Be("echo", "FileName was wrong.");
    }

    [Fact]
    public void Constructor_ConfiguresStartInfoForOutputCapture()
    {
        // arrange
        var startInfo = new ProcessStartInfo("echo", "hello");

        // act
        var sut = new ProcessRunner(startInfo);

        // assert
        sut.StartInfo.RedirectStandardOutput.Should().BeTrue("RedirectStandardOutput should be true.");
        sut.StartInfo.RedirectStandardError.Should().BeTrue("RedirectStandardError should be true.");
        sut.StartInfo.UseShellExecute.Should().BeFalse("UseShellExecute should be false.");
        sut.StartInfo.CreateNoWindow.Should().BeTrue("CreateNoWindow should be true.");
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
        actual.Should().Be(expectedTimeout, "Default timeout should be 10000ms.");
    }

    [Fact]
    public void Run_SuccessfulCommand_SetsIsSuccessTrue()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello world");

        // act
        var result = sut.Run();

        // assert
        sut.IsSuccess.Should().BeTrue("IsSuccess should be true for successful command.");
        sut.IsError.Should().BeFalse("IsError should be false for successful command.");
        sut.HasCompleted.Should().BeTrue("HasCompleted should be true.");
        result.Should().NotBeNull("Result should not be null.");
        result.IsSuccess.Should().BeTrue("Result.IsSuccess should be true.");
    }

    [Fact]
    public void Run_SuccessfulCommand_CapturesOutput()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello world");

        // act
        var result = sut.Run();

        // assert
        sut.OutputText.Should().Contain("hello world", "OutputText should contain the echoed text.");
        result.OutputText.Should().Contain("hello world", "Result.OutputText should contain the echoed text.");
    }

    [Fact]
    public void Run_SuccessfulCommand_ReturnsExitCodeZero()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");

        // act
        var result = sut.Run();

        // assert
        sut.ExitCode.Should().Be(0, "ExitCode should be 0 for successful command.");
        result.ExitCode.Should().Be(0, "Result.ExitCode should be 0.");
    }

    [Fact]
    public void Run_FailingCommand_SetsIsErrorTrue()
    {
        // arrange - use a command that will fail
        var sut = CreateSystemUnderTest("ls", "/nonexistent/directory/that/does/not/exist");

        // act
        var result = sut.Run();

        // assert
        sut.IsError.Should().BeTrue("IsError should be true for failing command.");
        sut.IsSuccess.Should().BeFalse("IsSuccess should be false for failing command.");
        sut.HasCompleted.Should().BeTrue("HasCompleted should be true.");
        result.IsError.Should().BeTrue("Result.IsError should be true.");
    }

    [Fact]
    public void Run_FailingCommand_ReturnsNonZeroExitCode()
    {
        // arrange
        var sut = CreateSystemUnderTest("ls", "/nonexistent/directory/that/does/not/exist");

        // act
        var result = sut.Run();

        // assert
        sut.ExitCode.Should().NotBe(0, "ExitCode should not be 0 for failing command.");
        result.ExitCode.Should().NotBe(0, "Result.ExitCode should not be 0.");
    }

    [Fact]
    public void Run_CalledTwice_ThrowsInvalidOperationException()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");
        sut.Run();

        // act & assert
        var action = () => sut.Run();
        action.Should().Throw<InvalidOperationException>("Should throw when Run() is called twice.");
    }

    [Fact]
    public void Run_WithTimeout_ThrowsTimeoutException()
    {
        // arrange - use sleep command with short timeout
        var sut = CreateSystemUnderTest("sleep", "10");
        sut.Timeout = 100; // 100ms timeout

        // act & assert
        var action = () => sut.Run();
        action.Should().Throw<TimeoutException>("Should throw TimeoutException when process times out.");
        sut.IsTimeout.Should().BeTrue("IsTimeout should be true after timeout.");
    }

    [Fact]
    public void ImplementsIProcessRunner()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");

        // act & assert
        sut.Should().BeAssignableTo<IProcessRunner>("ProcessRunner should implement IProcessRunner.");
    }

    [Fact]
    public void Run_ReturnsIProcessRunnerResult()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo", "hello");

        // act
        var result = sut.Run();

        // assert
        result.Should().BeAssignableTo<IProcessRunnerResult>("Run() should return IProcessRunnerResult.");
    }
}
