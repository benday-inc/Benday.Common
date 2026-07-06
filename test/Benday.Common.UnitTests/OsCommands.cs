using System;
using System.Runtime.InteropServices;

namespace Benday.Common.UnitTests;

/// <summary>
/// Provides OS-appropriate console commands so the ProcessRunner / AsyncProcessRunner tests
/// run on both Windows and Unix. On Windows these use built-in tools (<c>cmd.exe</c> and
/// <c>ping</c>); on Unix they use the standard shell utilities via <c>/bin/sh</c>.
/// Each factory returns a <c>(fileName, arguments)</c> pair suitable for
/// <see cref="System.Diagnostics.ProcessStartInfo"/>.
/// </summary>
internal static class OsCommands
{
    /// <summary>True when the tests are running on Windows.</summary>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    private static string CmdExe => Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe";

    /// <summary>
    /// A command that prints <paramref name="text"/> to standard output and exits with code 0.
    /// </summary>
    public static (string fileName, string arguments) Echo(string text) =>
        IsWindows
            ? (CmdExe, $"/c echo {text}")
            : ("/bin/sh", $"-c \"echo '{text}'\"");

    /// <summary>
    /// A command that runs for approximately <paramref name="seconds"/> seconds and exits with code 0.
    /// </summary>
    public static (string fileName, string arguments) Sleep(int seconds) =>
        IsWindows
            // ping sends N packets ~1s apart, so N+1 packets ≈ `seconds` of wall-clock time.
            ? (CmdExe, $"/c ping -n {seconds + 1} 127.0.0.1 >nul")
            : ("/bin/sh", $"-c \"sleep {seconds}\"");

    /// <summary>
    /// A command that exits with a non-zero exit code.
    /// </summary>
    public static (string fileName, string arguments) Failing() =>
        IsWindows
            ? (CmdExe, "/c exit 1")
            : ("/bin/sh", "-c \"exit 1\"");

    /// <summary>
    /// Prints <paramref name="first"/>, waits about <paramref name="seconds"/> seconds, then prints
    /// <paramref name="second"/>, and exits with code 0.
    /// </summary>
    public static (string fileName, string arguments) EchoWaitEcho(string first, int seconds, string second) =>
        IsWindows
            ? (CmdExe, $"/c echo {first}&ping -n {seconds + 1} 127.0.0.1 >nul&echo {second}")
            : ("/bin/sh", $"-c \"echo '{first}'; sleep {seconds}; echo '{second}'\"");
}
