using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Benday.Common.Testing;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests;

public class AsyncProcessRunnerFixture : TestClassBase
{
    public AsyncProcessRunnerFixture(ITestOutputHelper output) : base(output)
    {
    }

    private AsyncProcessRunner CreateSystemUnderTest(string fileName, string arguments = "")
    {
        var startInfo = new ProcessStartInfo(fileName, arguments);
        return new AsyncProcessRunner(startInfo);
    }

    [Fact]
    public void Constructor_SetsStartInfo()
    {
        // arrange
        var startInfo = new ProcessStartInfo("echo", "hello");

        // act
        var sut = new AsyncProcessRunner(startInfo);

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
        var sut = new AsyncProcessRunner(startInfo);

        // assert
        sut.StartInfo.RedirectStandardOutput.Should().BeTrue("RedirectStandardOutput should be true.");
        sut.StartInfo.RedirectStandardError.Should().BeTrue("RedirectStandardError should be true.");
        sut.StartInfo.UseShellExecute.Should().BeFalse("UseShellExecute should be false.");
        sut.StartInfo.CreateNoWindow.Should().BeTrue("CreateNoWindow should be true.");
    }

    [Fact]
    public void StartupTimeout_DefaultValue()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo");
        var expectedTimeout = 5000;

        // act
        var actual = sut.StartupTimeout;

        // assert
        actual.Should().Be(expectedTimeout, "Default StartupTimeout should be 5000ms.");
    }

    [Fact]
    public void RunTimeout_DefaultValue()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo");
        var expectedTimeout = 0;

        // act
        var actual = sut.RunTimeout;

        // assert
        actual.Should().Be(expectedTimeout, "Default RunTimeout should be 0 (no timeout).");
    }

    [Fact]
    public void InitialState_IsRunningFalse()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo");

        // act & assert
        sut.IsRunning.Should().BeFalse("IsRunning should be false before starting.");
        sut.HasStarted.Should().BeFalse("HasStarted should be false before starting.");
        sut.HasCompleted.Should().BeFalse("HasCompleted should be false before starting.");
    }

    [Fact]
    public void InitialState_ProcessIdIsNull()
    {
        // arrange
        var sut = CreateSystemUnderTest("echo");

        // act & assert
        sut.ProcessId.Should().BeNull("ProcessId should be null before starting.");
        sut.UnderlyingProcess.Should().BeNull("UnderlyingProcess should be null before starting.");
    }

    [Fact]
    public async Task StartAsync_StartsProcess()
    {
        // arrange
        using var sut = CreateSystemUnderTest("sleep", "1");

        // act
        await sut.StartAsync();

        // assert
        sut.HasStarted.Should().BeTrue("HasStarted should be true after StartAsync.");
        sut.IsRunning.Should().BeTrue("IsRunning should be true while process is running.");
        sut.ProcessId.Should().NotBeNull("ProcessId should not be null after starting.");
        sut.UnderlyingProcess.Should().NotBeNull("UnderlyingProcess should not be null after starting.");
    }

    [Fact]
    public async Task StartAsync_CalledTwice_ThrowsInvalidOperationException()
    {
        // arrange
        using var sut = CreateSystemUnderTest("sleep", "1");
        await sut.StartAsync();

        // act & assert
        var action = async () => await sut.StartAsync();
        await action.Should().ThrowAsync<InvalidOperationException>("Should throw when StartAsync() is called twice.");
    }

    [Fact]
    public async Task WaitForExitAsync_WaitsForCompletion()
    {
        // arrange
        using var sut = CreateSystemUnderTest("echo", "hello world");
        await sut.StartAsync();

        // act
        await sut.WaitForExitAsync();

        // assert
        sut.IsRunning.Should().BeFalse("IsRunning should be false after process completes.");
        sut.HasCompleted.Should().BeTrue("HasCompleted should be true after process completes.");
        sut.IsSuccess.Should().BeTrue("IsSuccess should be true for successful command.");
    }

    [Fact]
    public async Task WaitForExitAsync_BeforeStart_ThrowsInvalidOperationException()
    {
        // arrange
        using var sut = CreateSystemUnderTest("echo", "hello");

        // act & assert
        var action = async () => await sut.WaitForExitAsync();
        await action.Should().ThrowAsync<InvalidOperationException>("Should throw when WaitForExitAsync() is called before StartAsync().");
    }

    [Fact]
    public async Task SuccessfulCommand_CapturesOutput()
    {
        // arrange
        using var sut = CreateSystemUnderTest("echo", "hello async world");
        await sut.StartAsync();

        // act
        await sut.WaitForExitAsync();

        // Small delay to ensure output buffer is fully captured
        await Task.Delay(100);

        // assert
        sut.OutputText.Should().Contain("hello async world", "OutputText should contain the echoed text.");
    }

    [Fact]
    public async Task SuccessfulCommand_ReturnsExitCodeZero()
    {
        // arrange
        using var sut = CreateSystemUnderTest("echo", "hello");
        await sut.StartAsync();

        // act
        await sut.WaitForExitAsync();

        // assert
        sut.ExitCode.Should().Be(0, "ExitCode should be 0 for successful command.");
    }

    [Fact]
    public async Task FailingCommand_SetsIsErrorTrue()
    {
        // arrange
        using var sut = CreateSystemUnderTest("ls", "/nonexistent/directory/that/does/not/exist");
        await sut.StartAsync();

        // act
        await sut.WaitForExitAsync();

        // assert
        sut.IsError.Should().BeTrue("IsError should be true for failing command.");
        sut.IsSuccess.Should().BeFalse("IsSuccess should be false for failing command.");
        sut.ExitCode.Should().NotBe(0, "ExitCode should not be 0 for failing command.");
    }

    [Fact]
    public async Task Kill_TerminatesRunningProcess()
    {
        // arrange
        using var sut = CreateSystemUnderTest("sleep", "30");
        await sut.StartAsync();
        sut.IsRunning.Should().BeTrue("Process should be running.");

        // act
        sut.Kill();
        await Task.Delay(500); // Give it time to clean up

        // assert
        sut.IsRunning.Should().BeFalse("IsRunning should be false after Kill().");
    }

    [Fact]
    public async Task RunTimeout_TerminatesLongRunningProcess()
    {
        // arrange
        using var sut = CreateSystemUnderTest("sleep", "30");
        sut.RunTimeout = 500; // 500ms timeout

        // act
        await sut.StartAsync();
        await sut.WaitForExitAsync();

        // assert
        sut.IsTimeout.Should().BeTrue("IsTimeout should be true when process times out.");
        sut.IsRunning.Should().BeFalse("IsRunning should be false after timeout.");
    }

    [Fact]
    public async Task OutputText_CanBeReadWhileRunning()
    {
        // arrange - use a command that outputs something before sleeping
        using var sut = CreateSystemUnderTest("/bin/bash", "-c \"echo 'start'; sleep 2; echo 'end'\"");
        await sut.StartAsync();

        // act - wait a bit for first output
        await Task.Delay(500);
        var outputWhileRunning = sut.OutputText;

        // assert
        sut.IsRunning.Should().BeTrue("Process should still be running.");
        outputWhileRunning.Should().Contain("start", "Should be able to read output while process is running.");

        // cleanup
        sut.Kill();
    }

    [Fact]
    public async Task Dispose_KillsRunningProcess()
    {
        // arrange
        var sut = CreateSystemUnderTest("sleep", "30");
        await sut.StartAsync();
        var processId = sut.ProcessId;
        processId.Should().NotBeNull("ProcessId should be set.");

        // act
        sut.Dispose();
        await Task.Delay(500); // Give it time to clean up

        // assert - process should no longer be running
        try
        {
            var process = Process.GetProcessById(processId!.Value);
            process.HasExited.Should().BeTrue("Process should have exited after Dispose().");
        }
        catch (ArgumentException)
        {
            // Process doesn't exist anymore, which is expected
        }
    }

    [Fact]
    public void ImplementsIAsyncProcessRunner()
    {
        // arrange
        using var sut = CreateSystemUnderTest("echo", "hello");

        // act & assert
        sut.Should().BeAssignableTo<IAsyncProcessRunner>("AsyncProcessRunner should implement IAsyncProcessRunner.");
    }

    [Fact]
    public void ImplementsIDisposable()
    {
        // arrange
        using var sut = CreateSystemUnderTest("echo", "hello");

        // act & assert
        sut.Should().BeAssignableTo<IDisposable>("AsyncProcessRunner should implement IDisposable.");
    }

    [Fact]
    public async Task CancellationToken_CancelsWaitForExit()
    {
        // arrange
        using var sut = CreateSystemUnderTest("sleep", "30");
        using var cts = new CancellationTokenSource(500); // Cancel after 500ms
        await sut.StartAsync();

        // act & assert
        var action = async () => await sut.WaitForExitAsync(cts.Token);
        await action.Should().ThrowAsync<TaskCanceledException>("Should throw when cancellation is requested.");

        // cleanup
        sut.Kill();
    }
}
