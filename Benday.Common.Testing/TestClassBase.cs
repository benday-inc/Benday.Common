using System.Runtime.CompilerServices;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.Testing;

public abstract class TestClassBase
{
    private readonly ITestOutputHelper _output;

    public TestClassBase(ITestOutputHelper output)
    {
        _output = output;
    }

    public void WriteLine(string message)
    {
        _output.WriteLine(message);
    }

    protected virtual string SampleFilesDirectoryName
    {
        get
        {
            return "sample-files";
        }
    }

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

    protected string GetPathToTestFile(
        [CallerFilePath] string callerFile = "")
    {
        return callerFile;
    }

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
