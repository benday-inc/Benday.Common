namespace Benday.Common
{
    /// <summary>
    /// Represents the result of a process execution.
    /// </summary>
    public class ProcessRunnerResult : IProcessRunnerResult
    {
        private const int EXIT_CODE_NOT_SET = -1;

        /// <summary>
        /// Creates a new instance of ProcessRunnerResult with default values.
        /// </summary>
        public ProcessRunnerResult()
        {
            ExitCode = EXIT_CODE_NOT_SET;
            OutputText = string.Empty;
            ErrorText = string.Empty;
        }

        /// <summary>
        /// Creates a new instance of ProcessRunnerResult with the specified values.
        /// </summary>
        /// <param name="isError">Indicates if the process completed with an error.</param>
        /// <param name="isSuccess">Indicates if the process completed successfully.</param>
        /// <param name="isTimeout">Indicates if the process timed out.</param>
        /// <param name="exitCode">The exit code from the process.</param>
        /// <param name="outputText">The standard output text from the process.</param>
        /// <param name="errorText">The standard error text from the process.</param>
        public ProcessRunnerResult(
            bool isError,
            bool isSuccess,
            bool isTimeout,
            int exitCode,
            string outputText,
            string errorText)
        {
            IsError = isError;
            IsSuccess = isSuccess;
            IsTimeout = isTimeout;
            ExitCode = exitCode;
            OutputText = outputText;
            ErrorText = errorText;
        }

        /// <summary>
        /// Indicates if the process completed with an error (non-zero exit code).
        /// </summary>
        public bool IsError { get; internal set; }

        /// <summary>
        /// Indicates if the process completed successfully (zero exit code).
        /// </summary>
        public bool IsSuccess { get; internal set; }

        /// <summary>
        /// Indicates if the process timed out.
        /// </summary>
        public bool IsTimeout { get; internal set; }

        /// <summary>
        /// Indicates if the process has completed (either successfully or with error).
        /// </summary>
        public bool HasCompleted => IsError || IsSuccess;

        /// <summary>
        /// The exit code from the process. Returns -1 if not yet completed.
        /// </summary>
        public int ExitCode { get; internal set; }

        /// <summary>
        /// The standard output text from the process.
        /// </summary>
        public string OutputText { get; internal set; }

        /// <summary>
        /// The standard error text from the process.
        /// </summary>
        public string ErrorText { get; internal set; }
    }
}
