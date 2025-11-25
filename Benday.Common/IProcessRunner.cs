using System.Diagnostics;

namespace Benday.Common
{
    /// <summary>
    /// Interface for running external processes synchronously.
    /// </summary>
    public interface IProcessRunner
    {
        /// <summary>
        /// The ProcessStartInfo object used to configure the process.
        /// </summary>
        ProcessStartInfo StartInfo { get; }

        /// <summary>
        /// The timeout in milliseconds for the process to run.
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Run the process synchronously and wait for completion.
        /// </summary>
        /// <returns>The result of the process execution.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if Run() is called more than once.</exception>
        /// <exception cref="System.TimeoutException">Thrown if the process times out.</exception>
        IProcessRunnerResult Run();

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
        /// The output text from the process.
        /// </summary>
        string OutputText { get; }

        /// <summary>
        /// The error text from the process.
        /// </summary>
        string ErrorText { get; }
    }
}
