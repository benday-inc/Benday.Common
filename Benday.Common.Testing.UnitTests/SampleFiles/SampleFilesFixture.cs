using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Benday.Common.Testing;

using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.SampleFiles;
public class SampleFilesFixture : TestClassBase
{
    public SampleFilesFixture(ITestOutputHelper output) : base(output)
    {
        
    }

    [Fact]
    public void ValidateSampleFilesDirectoryName()
    {
        var expected = "sample-files";
        var actual = SampleFilesDirectoryName;

        Assert.Equal(expected, actual);
    }

    private string GetPathToExpectedSampleFilesDirectory()
    {
        var pathToUnitTestFile = GetPathToTestFile();

        var pathToUnitTestDirectory = Path.GetDirectoryName(pathToUnitTestFile);

        if (string.IsNullOrEmpty(pathToUnitTestDirectory) == true)
        {
            Assert.Fail($"Path to unit test directory is null or empty.");
        }

        var unitTestDir = new DirectoryInfo(pathToUnitTestDirectory);

        Assert.True(unitTestDir.Exists, $"Unit test directory '{unitTestDir.FullName}' does not exist.");

        var parentDir = unitTestDir.Parent;

        Assert.True(parentDir != null, $"Parent directory of unit test directory '{unitTestDir.FullName}' is null.");

        var sampleFilesDir = new DirectoryInfo(Path.Combine(parentDir.FullName, SampleFilesDirectoryName));

        return sampleFilesDir.FullName;
    }

    [Fact]
    public void GetPathToSampleDirReturnsExpected()
    {
        var expected = GetPathToExpectedSampleFilesDirectory();

        var actual = GetPathToSampleFilesDirectory();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetSampleFilePathReturnsExpectedPath()
    {
        var fileName = "sample-file.txt";
        var expectedDir = GetPathToExpectedSampleFilesDirectory();

        var expected = Path.Combine(
            expectedDir,
            fileName);

        var actual = GetSampleFilePath(fileName);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetSampleFileTextReturnsExpectedContents()
    {
        var fileName = "sample-file.txt";
        var expected = "Hello World!";

        var actual = GetSampleFileText(fileName);

        Assert.Equal(expected, actual);
    }
}
