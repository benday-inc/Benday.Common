using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Benday.Common
{
    /// <summary>
    /// Interface for running external processes asynchronously in the background.
    /// Supports monitoring state while running and provides process control.
    /// </summary>
    public interface IAsyncProcessRunner : IDisposable
    {
        /// <summary>
        /// The ProcessStartInfo object used to configure the process.
        /// </summary>
        ProcessStartInfo StartInfo { get; }

        /// <summary>
        /// The timeout in milliseconds for the process startup.
        /// The process must start within this time or a TimeoutException is thrown.
        /// Default is 5000ms (5 seconds).
        /// </summary>
        int StartupTimeout { get; set; }

        /// <summary>
        /// The timeout in milliseconds for the overall process run duration.
        /// If exceeded, the process is terminated and IsTimeout becomes true.
        /// Default is 0 (no timeout - runs indefinitely).
        /// </summary>
        int RunTimeout { get; set; }

        /// <summary>
        /// Start the process in the background (non-blocking).
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Task that completes when the process has started.</returns>
        /// <exception cref="InvalidOperationException">Thrown if StartAsync() is called more than once.</exception>
        /// <exception cref="TimeoutException">Thrown if the process does not start within StartupTimeout.</exception>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Wait for the process to complete.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Task that completes when the process has finished.</returns>
        Task WaitForExitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Kill/stop the running process.
        /// </summary>
        /// <param name="entireProcessTree">If true, kills the entire process tree.</param>
        void Kill(bool entireProcessTree = false);

        /// <summary>
        /// Indicates if the process is currently running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Indicates if the process has been started.
        /// </summary>
        bool HasStarted { get; }

        /// <summary>
        /// Indicates if the process completed with an error.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Indicates if the process completed successfully.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Indicates if the process timed out.
        /// </summary>
        bool IsTimeout { get; }

        /// <summary>
        /// Indicates if the process has completed.
        /// </summary>
        bool HasCompleted { get; }

        /// <summary>
        /// The process ID. Returns null if process has not started.
        /// </summary>
        int? ProcessId { get; }

        /// <summary>
        /// Direct access to the underlying Process instance.
        /// Returns null if process has not started.
        /// Use with caution - prefer using the interface methods.
        /// </summary>
        Process? UnderlyingProcess { get; }

        /// <summary>
        /// The exit code from the process. Returns -1 if not yet completed.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// Get the current output text. Can be called while process is running.
        /// </summary>
        string OutputText { get; }

        /// <summary>
        /// Get the current error text. Can be called while process is running.
        /// </summary>
        string ErrorText { get; }
    }
}
