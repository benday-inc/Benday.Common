using System.Runtime.CompilerServices;

using Xunit;

namespace Benday.Common.Testing;

/// <summary>
/// Base class for XUnit-based test classes.
/// </summary>
public abstract class TestClassBase
{
    private readonly ITestOutputHelper _output;

    /// <summary>
    /// Constructor for the base class.
    /// </summary>
    /// <param name="output">An instance of the XUnit ITestOutputHelper. This typically is provided by the XUnit runner itself at execution.</param>
    public TestClassBase(ITestOutputHelper output)
    {
        _output = output;
    }

    /// <summary>
    /// Write a message to the test output.
    /// </summary>
    /// <param name="message"></param>
    public void WriteLine(string message)
    {
        _output.WriteLine(message);
    }

    /// <summary>
    /// The name of the directory that contains sample files used by tests.
    /// Defaults to "sample-files". Override this property to use a different
    /// directory name.
    /// </summary>
    protected virtual string SampleFilesDirectoryName
    {
        get
        {
            return "sample-files";
        }
    }

    /// <summary>
    /// Gets the full path to a sample file with the given file name by locating
    /// the sample files directory and combining it with the file name. Fails the
    /// test if the sample files directory cannot be found.
    /// </summary>
    /// <param name="fileName">The name of the sample file.</param>
    /// <returns>The full path to the sample file.</returns>
    protected virtual string GetSampleFilePath(string fileName)
    {
        var pathToSampleFiles = GetPathToSampleFilesDirectory();
        if (string.IsNullOrEmpty(pathToSampleFiles) == true)
        {
            Assert.Fail($"Could not find sample files directory '{SampleFilesDirectoryName}'");
        }
        var pathToFile = Path.Combine(pathToSampleFiles, fileName);
        return pathToFile;
    }

    /// <summary>
    /// Reads and returns the text contents of the sample file with the given
    /// file name. Fails the test if the sample file cannot be located.
    /// </summary>
    /// <param name="fileName">The name of the sample file.</param>
    /// <returns>The text contents of the sample file.</returns>
    protected virtual string GetSampleFileText(string fileName)
    {
        var pathToFile = GetSampleFilePath(fileName);
        if (string.IsNullOrEmpty(pathToFile) == true)
        {
            Assert.Fail($"Could not find sample file '{fileName}'");
        }
        var text = File.ReadAllText(pathToFile);
        return text;
    }

    /// <summary>
    /// Returns the path to the source file of the calling test. The path is
    /// supplied automatically by the compiler via the <see cref="CallerFilePathAttribute"/>,
    /// so callers should not provide a value.
    /// </summary>
    /// <param name="callerFile">The caller's source file path, populated automatically by the compiler.</param>
    /// <returns>The full path to the calling test's source file.</returns>
    protected string GetPathToTestFile(
        [CallerFilePath] string callerFile = "")
    {
        return callerFile;
    }

    /// <summary>
    /// Locates the sample files directory by starting at the directory of the
    /// current test assembly and searching upward through parent directories.
    /// Fails the test if the directory cannot be found.
    /// </summary>
    /// <returns>The full path to the sample files directory.</returns>
    protected virtual string GetPathToSampleFilesDirectory()
    {
        var pathToAssembly = GetType().Assembly.Location;

        var dirToCheck = Path.GetDirectoryName(pathToAssembly);

        if (string.IsNullOrEmpty(dirToCheck) == true)
        {
            Assert.Fail("Could not determine path to assembly");
        }
        else
        {
            var sampleFilesDir = GetPathToSampleFilesDirectory(dirToCheck);

            if (string.IsNullOrEmpty(sampleFilesDir) == true)
            {
                Assert.Fail($"Could not find sample files directory '{SampleFilesDirectoryName}'");
            }

            return sampleFilesDir;
        }

        return string.Empty;
    }

    /// <summary>
    /// Searches for the sample files directory starting at the given directory
    /// and walking upward through parent directories until it is found.
    /// </summary>
    /// <param name="startingDir">The directory to begin searching from.</param>
    /// <returns>The full path to the sample files directory, or null if it cannot be found.</returns>
    protected string? GetPathToSampleFilesDirectory(string startingDir)
    {
        var dirToCheck = new DirectoryInfo(startingDir);

        var sampleFilesDirName = SampleFilesDirectoryName;

        while (dirToCheck != null && dirToCheck.Exists == true)
        {
            var pathToSampleFiles = Path.Combine(dirToCheck.FullName, sampleFilesDirName);

            var sampleFilesDir = new DirectoryInfo(pathToSampleFiles);

            if (sampleFilesDir.Exists == true)
            {
                return sampleFilesDir.FullName;
            }
            else
            {
                dirToCheck = dirToCheck.Parent;
            }
        }

        return null;
    }
}
