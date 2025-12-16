using System.Linq;
using System.Reflection;

using Benday.Common.Testing;

using Microsoft.Extensions.Logging;


using Xunit;
using Xunit.Abstractions;

namespace Benday.Common.UnitTests.MockingUtilities;


public class MockUtilityFixture : TestClassBase
{
    public MockUtilityFixture(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CreateInstanceWithMocks_DefaultConstructor()
    {
        var result = MockUtility.CreateInstance<ClassWithDefaultConstructor>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(0, "no mocks should be created for this instance");
        result.Instance.ShouldNotBeNull("Instance was null");
    }

    [Fact]
    public void CreateInstanceWithMocks_ConstructorTakesNoArgs()
    {
        var result = MockUtility.CreateInstance<ClassWithEmptyConstructor>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(0, "no mocks should be created for this instance");
        result.Instance.ShouldNotBeNull("Instance was null");
    }

    [Fact]
    public void CreateInstanceWithMocks_ConstructorTakesOneInterface()
    {
        var result = MockUtility.CreateInstance<ClassWithOneDependency>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "a mock should be created for this instance");
        result.Instance.ShouldNotBeNull("Instance was null");

        var mock0 = result.Mocks.FirstOrDefault();

        mock0.Key.ShouldNotBeNull("mock0.Key should not be null");
        result.Instance.Repository.ShouldBeSameAs(mock0.Value.Object, "Values didn't match");
        mock0.Key.ShouldEqual(typeof(ISampleRepository), "Key didn't match");
    }

    [Fact]
    public void CreateInstanceWithMocks_ConstructorTakesMultipleParameters()
    {
        var result = MockUtility.CreateInstance<ClassWithMultipleDependencies>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(2, "mocks should be created for this instance");
        result.Instance.ShouldNotBeNull("Instance was null");

        var mock0 = result.Mocks.FirstOrDefault();
        var mock1 = result.Mocks.LastOrDefault();

        mock0.Key.ShouldNotBeNull("mock0.Key should not be null");
        mock1.Key.ShouldNotBeNull("mock1.Key should not be null");

        result.Instance.Repository.ShouldBeSameAs(mock0.Value.Object, "Values didn't match for item 0");
        mock0.Key.ShouldEqual(typeof(ISampleRepository), "Key didn't match for item 0");

        result.Instance.Logger.ShouldBeSameAs(mock1.Value.Object, "Values didn't match for item 1");
        mock1.Key.ShouldEqual(typeof(ILogger<ClassWithMultipleDependencies>), "Key didn't match for item 1");
    }

    [Fact]
    public void MockCreationResult_GetMockByType()
    {
        var result = MockUtility.CreateInstance<ClassWithMultipleDependencies>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(2, "mocks should be created for this instance");

        var mockOfSlideDataRepository = result.GetMock<ISampleRepository>();
        mockOfSlideDataRepository.ShouldNotBeNull("mockOfSlideDataRepository should not be null");

        var mockOfLogger = result.GetMock<ILogger<ClassWithMultipleDependencies>>();
        mockOfLogger.ShouldNotBeNull("mockOfLogger should not be null");
    }

    [Fact]
    public void MockCreationResult_GetMockByType_BogusDependency()
    {
        var result = MockUtility.CreateInstance<ClassWithDefaultConstructor>();

        result.ShouldNotBeNull("Result should not be null");

        var mockOfLogger = result.GetMock<ILogger<ClassWithMultipleDependencies>>();
        mockOfLogger.ShouldBeNull("mockOfLogger should be null");
    }

    [Fact]
    public void Instance_IsLazyLoaded_WithConfiguredMocks()
    {
        var result = MockUtility.CreateInstance<ClassWithOneDependencyAndValidationLogicInTheConstructor>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "a mock should be created for this instance");

        result.IsInstanceCreated.ShouldBeFalse("Instance should not be created yet");

        var mock = result.GetRequiredMock<ISampleConfigurationInfo>();

        mock.SetupGet(x => x.ConfigurationValue).Returns("Test Value");

        result.IsInstanceCreated.ShouldBeFalse("Instance should still not be created yet");

        result.Instance.ShouldNotBeNull("Instance was null");

        result.IsInstanceCreated.ShouldBeTrue("Instance should be created");
    }

    [Fact]
    public void Instance_IsLazyLoaded_WithoutConfiguredMocks()
    {
        var result = MockUtility.CreateInstance<ClassWithOneDependencyAndValidationLogicInTheConstructor>();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "a mock should be created for this instance");

        result.IsInstanceCreated.ShouldBeFalse("Instance should not be created yet");

        var ex = Assert.Throws<TargetInvocationException>(() =>
        {
            // this should throw an exception from the constructor validation logic
            // because we haven't configured the mock yet
            var instance = result.Instance;
        });

        ex.InnerException.ShouldBeOfType<ArgumentException>("InnerException should be ArgumentException");
        ex.InnerException!.Message.ShouldContain("Configuration value cannot be null or empty.", "Exception message should contain expected text");
    }
}
