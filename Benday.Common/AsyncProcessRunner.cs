using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Benday.Common
{
    /// <summary>
    /// Runs a process asynchronously in the background.
    /// Supports monitoring state while running, startup/run timeouts, and process control.
    /// </summary>
    public class AsyncProcessRunner : IAsyncProcessRunner
    {
        private const int DEFAULT_STARTUP_TIMEOUT_MS = 5000;
        private const int DEFAULT_RUN_TIMEOUT_MS = 0; // No timeout
        private const int EXIT_CODE_SUCCESS = 0;
        private const int EXIT_CODE_NOT_SET = -1;

        private Process? _process;
        private readonly StringBuilder _outputBuilder = new StringBuilder();
        private readonly StringBuilder _errorBuilder = new StringBuilder();
        private readonly object _lock = new object();
        private bool _hasStartBeenCalled = false;
        private bool _isDisposed = false;
        private TaskCompletionSource<bool>? _exitTaskSource;
        private CancellationTokenSource? _runTimeoutCts;

        /// <summary>
        /// Creates a new instance of AsyncProcessRunner.
        /// </summary>
        /// <param name="startInfo">The ProcessStartInfo to configure the process.</param>
        public AsyncProcessRunner(ProcessStartInfo startInfo)
        {
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            StartInfo = startInfo;
        }

        /// <summary>
        /// The ProcessStartInfo object used to configure the process.
        /// </summary>
        public ProcessStartInfo StartInfo { get; private set; }

        /// <summary>
        /// The timeout in milliseconds for the process startup.
        /// Default is 5000ms (5 seconds).
        /// </summary>
        public int StartupTimeout { get; set; } = DEFAULT_STARTUP_TIMEOUT_MS;

        /// <summary>
        /// The timeout in milliseconds for the overall process run duration.
        /// Default is 0 (no timeout - runs indefinitely).
        /// </summary>
        public int RunTimeout { get; set; } = DEFAULT_RUN_TIMEOUT_MS;

        /// <summary>
        /// Indicates if the process is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Indicates if the process has been started.
        /// </summary>
        public bool HasStarted { get; private set; }

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
        /// </summary>
        public bool HasCompleted => IsError || IsSuccess;

        /// <summary>
        /// The process ID. Returns null if process has not started.
        /// </summary>
        public int? ProcessId => _process?.Id;

        /// <summary>
        /// Direct access to the underlying Process instance.
        /// </summary>
        public Process? UnderlyingProcess => _process;

        /// <summary>
        /// The exit code from the process. Returns -1 if not yet completed.
        /// </summary>
        public int ExitCode { get; private set; } = EXIT_CODE_NOT_SET;

        /// <summary>
        /// Get the current output text. Thread-safe, can be called while process is running.
        /// </summary>
        public string OutputText
        {
            get
            {
                lock (_lock)
                {
                    return _outputBuilder.ToString();
                }
            }
        }

        /// <summary>
        /// Get the current error text. Thread-safe, can be called while process is running.
        /// </summary>
        public string ErrorText
        {
            get
            {
                lock (_lock)
                {
                    return _errorBuilder.ToString();
                }
            }
        }

        /// <summary>
        /// Start the process in the background (non-blocking).
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Task that completes when the process has started.</returns>
        /// <exception cref="InvalidOperationException">Thrown if StartAsync() is called more than once.</exception>
        /// <exception cref="TimeoutException">Thrown if the process does not start within StartupTimeout.</exception>
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (_hasStartBeenCalled)
            {
                throw new InvalidOperationException("Cannot call StartAsync a second time.");
            }

            _hasStartBeenCalled = true;
            _exitTaskSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            _process = new Process();
            _process.StartInfo = StartInfo ??
                throw new InvalidOperationException("StartInfo was null");

            _process.EnableRaisingEvents = true;

            _process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    lock (_lock)
                    {
                        _outputBuilder.AppendLine(e.Data);
                    }
                }
            };

            _process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    lock (_lock)
                    {
                        _errorBuilder.AppendLine(e.Data);
                    }
                }
            };

            _process.Exited += (sender, e) =>
            {
                FinalizeResult();
                _exitTaskSource?.TrySetResult(true);
            };

            // Start the process with startup timeout
            using var startupCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if (StartupTimeout > 0)
            {
                startupCts.CancelAfter(StartupTimeout);
            }

            try
            {
                await Task.Run(() =>
                {
                    startupCts.Token.ThrowIfCancellationRequested();

                    if (!_process.Start())
                    {
                        throw new InvalidOperationException("Process failed to start.");
                    }

                    HasStarted = true;
                    IsRunning = true;

                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();
                }, startupCts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Process failed to start within {StartupTimeout} milliseconds.");
            }

            // Setup run timeout if specified
            if (RunTimeout > 0)
            {
                _runTimeoutCts = new CancellationTokenSource(RunTimeout);
                _runTimeoutCts.Token.Register(() =>
                {
                    if (IsRunning)
                    {
                        IsTimeout = true;
                        Kill(entireProcessTree: true);
                    }
                });
            }
        }

        /// <summary>
        /// Wait for the process to complete.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Task that completes when the process has finished.</returns>
        public async Task WaitForExitAsync(CancellationToken cancellationToken = default)
        {
            if (_process == null || _exitTaskSource == null)
            {
                throw new InvalidOperationException("Process has not been started. Call StartAsync first.");
            }

            if (!IsRunning && HasCompleted)
            {
                return; // Already completed
            }

            using var registration = cancellationToken.Register(() =>
                _exitTaskSource.TrySetCanceled());

            await _exitTaskSource.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Kill/stop the running process.
        /// </summary>
        /// <param name="entireProcessTree">If true, kills the entire process tree.</param>
        public void Kill(bool entireProcessTree = false)
        {
            if (_process == null)
            {
                return;
            }

            if (!IsRunning)
            {
                return;
            }

            try
            {
#if NETSTANDARD2_1
                _process.Kill();
#else
                _process.Kill(entireProcessTree);
#endif
            }
            catch (InvalidOperationException)
            {
                // Process already exited, ignore
            }
            catch (SystemException)
            {
                // Process may have exited between check and kill, ignore
            }
        }

        private void FinalizeResult()
        {
            IsRunning = false;
            _runTimeoutCts?.Dispose();
            _runTimeoutCts = null;

            if (_process == null)
            {
                return;
            }

            try
            {
                ExitCode = _process.ExitCode;

                if (ExitCode != EXIT_CODE_SUCCESS)
                {
                    IsError = true;
                    IsSuccess = false;
                }
                else
                {
                    IsError = false;
                    IsSuccess = true;
                }
            }
            catch (InvalidOperationException)
            {
                // Process may not have exited properly
                IsError = true;
                IsSuccess = false;
            }
        }

        /// <summary>
        /// Disposes of the AsyncProcessRunner and kills the process if still running.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if from finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (IsRunning)
                {
                    try
                    {
                        Kill(entireProcessTree: true);
                    }
                    catch
                    {
                        // Ignore errors during disposal
                    }
                }

                _runTimeoutCts?.Dispose();
                _process?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
