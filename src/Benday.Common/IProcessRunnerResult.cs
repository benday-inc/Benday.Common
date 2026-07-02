namespace Benday.Common
{
    /// <summary>
    /// Represents the result of a process execution.
    /// </summary>
    public interface IProcessRunnerResult
    {
        /// <summary>
        /// Indicates if the process completed with an error (non-zero exit code).
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Indicates if the process completed successfully (zero exit code).
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Indicates if the process timed out.
        /// </summary>
        bool IsTimeout { get; }

        /// <summary>
        /// Indicates if the process has completed (either successfully or with error).
        /// </summary>
        bool HasCompleted { get; }

        /// <summary>
        /// The exit code from the process. Returns -1 if not yet completed.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// The standard output text from the process.
        /// </summary>
        string OutputText { get; }

        /// <summary>
        /// The standard error text from the process.
        /// </summary>
        string ErrorText { get; }
    }
}
