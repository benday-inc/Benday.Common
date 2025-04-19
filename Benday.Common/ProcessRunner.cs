using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Benday.Common;

/// <summary>
/// This class is used to run a process and capture the output.
/// It is a wrapper around the System.Diagnostics.Process class.
/// </summary>
public class ProcessRunner
{
    private const int TIMEOUT_IN_MILLISECS = 10000;
    private const int EXIT_CODE_SUCCESS = 0;
    private const int EXIT_CODE_NOT_SET = -1;

    public ProcessRunner(ProcessStartInfo startInfo)
    {
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        StartInfo = startInfo;
    }

    /// <summary>
    /// The ProcessStartInfo object that is used to start the process.
    /// This object is used to configure the process start info before running.
    /// </summary>
    public ProcessStartInfo StartInfo { get; private set; }

    private bool _hasRunBeenCalled = false;

    /// <summary>
    /// Run the command
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TimeoutException"></exception>
    public void Run()
    {
        if (_hasRunBeenCalled == true)
        {
            throw new InvalidOperationException($"Cannot call run a second time.");
        }

        _hasRunBeenCalled = true;
        
        using (var process = new Process())
        {
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.StartInfo = StartInfo ??
                throw new InvalidOperationException(
                    "StartInfo was null");

            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        outputBuilder.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        errorBuilder.AppendLine(e.Data);
                    }
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                var exitCode = EXIT_CODE_NOT_SET;

                if (
                    process.WaitForExit(Timeout) &&
                    outputWaitHandle.WaitOne(Timeout) &&
                    errorWaitHandle.WaitOne(Timeout))
                {
                    // Process completed. Check process.ExitCode here.
                    exitCode = process.ExitCode;
                }
                else
                {
                    SetResultData(true, outputBuilder, errorBuilder);

                    IsTimeout = true;

                    throw new TimeoutException(
                        $"Process timed out after {Timeout} milliseconds.");
                }

                if (process.ExitCode != EXIT_CODE_SUCCESS)
                {
                    SetResultData(true, outputBuilder, errorBuilder);
                }
                else
                {
                    SetResultData(false, outputBuilder, errorBuilder);
                }
            }
        }
    }

    /// <summary>
    /// The timeout in milliseconds for the process to run.
    /// The default timeout is 10 seconds.
    /// </summary>
    public int Timeout
    {
        get;
        set;
    } = TIMEOUT_IN_MILLISECS;

    private void SetResultData(
        bool isError,
        StringBuilder outputBuilder, StringBuilder errorBuilder)
    {
        if (isError == true)
        {
            IsError = true;
            IsSuccess = false;
        }
        else
        {
            IsError = false;
            IsSuccess = true;
        }

        OutputText = outputBuilder.ToString();
        ErrorText = errorBuilder.ToString();
    }

    /// <summary>
    /// Indicates if the process completed with an error.
    /// </summary>
    public bool IsError { get; private set; }

    /// <summary>
    /// Indicates if the process completed successfully.
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Indicates if the process timed out.
    /// </summary>
    public bool IsTimeout { get; private set; }

    /// <summary>
    /// Indicates if the process has completed.
    /// This is true if the process has completed with an error or
    /// successfully.
    /// </summary>
    public bool HasCompleted { get => IsError | IsSuccess; }

    /// <summary>
    /// The output text from the process.
    /// This is the standard output from the process.
    /// </summary>
    public string OutputText { get; private set; } = string.Empty;

    /// <summary>
    /// The error text from the process.
    /// This is the standard error from the process.
    /// </summary>
    public string ErrorText { get; private set; } = string.Empty;
}
