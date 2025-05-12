
using Benday.Common.Testing;

using System.IO;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Benday.Common.UnitTests.SampleFiles;

public class SampleFilesWithCustomSampleFilesDirNameFixture : TestClassBase
{
    public SampleFilesWithCustomSampleFilesDirNameFixture(ITestOutputHelper output) : base(output)
    {

    }

    protected override string SampleFilesDirectoryName => "sample-files-2";


    [Fact]
    public void ValidateSampleFilesDirectoryName()
    {
        var expected = "sample-files-2";
        var actual = SampleFilesDirectoryName;

        Assert.Equal(expected, actual);
    }

    [Fact]    
    public void GetPathToSampleDir_ThrowsException()
    {
        var expectedErrorMessage = "Could not find sample files directory 'sample-files-2'";

        var ex = Assert.Throws<FailException>(() =>
        {
            var pathToSampleFiles = GetPathToSampleFilesDirectory();            
        });

        Assert.Equal(expectedErrorMessage, ex.Message);
    }
}
