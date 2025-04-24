using Microsoft.Extensions.Logging;
using Moq;
using PowerpointUtil.Api.Repositories;
using System;
using System.Linq;

namespace Benday.Common.UnitTests.MockingUtilities;


[TestClass]
public class MockUtilityFixture
{
    public MockUtilityFixture()
    {
    }

    [TestMethod]
    public void CreateInstanceWithMocks_DefaultConstructor()
    {
        var result = MockUtility.CreateInstance<ClassWithDefaultConstructor>();

        Assert.IsNotNull(result);

        Assert.AreEqual(0, result.Mocks.Count, $"Should not be any mocks created for this instance.");

        Assert.IsNotNull(result.Instance, "Instance was null");
    }

    [TestMethod]
    public void CreateInstanceWithMocks_ConstructorTakesNoArgs()
    {
        var result = MockUtility.CreateInstance<ClassWithEmptyConstructor>();

        Assert.IsNotNull(result);

        Assert.AreEqual(0, result.Mocks.Count, $"Should not be any mocks created for this instance.");

        Assert.IsNotNull(result.Instance, "Instance was null");
    }

    [TestMethod]
    public void CreateInstanceWithMocks_ConstructorTakesOneInterface()
    {
        var result = MockUtility.CreateInstance<ClassWithOneDependency>();

        Assert.IsNotNull(result);

        Assert.AreEqual(1, result.Mocks.Count, $"Should be mocks created for this instance.");

        Assert.IsNotNull(result.Instance, "Instance was null");

        var mock0 = result.Mocks.FirstOrDefault();

        Assert.IsNotNull(mock0);

        Assert.AreSame(result.Instance.Repository, mock0.Value.Object, "Values didn't match");
        Assert.AreEqual(typeof(ISampleRepository), mock0.Key, "Key didn't match");
    }

    [TestMethod]
    public void CreateInstanceWithMocks_ConstructorTakesMultipleParameters()
    {
        var result = MockUtility.CreateInstance<ClassWithMultipleDependencies>();

        Assert.IsNotNull(result);

        Assert.AreEqual(2, result.Mocks.Count, $"Should be mocks created for this instance.");

        Assert.IsNotNull(result.Instance, "Instance was null");

        // get the first mock
        var mock0 = result.Mocks.FirstOrDefault();

        // get the second mock
        var mock1 = result.Mocks.LastOrDefault();

        Assert.IsNotNull(mock0);
        Assert.IsNotNull(mock1);

        Assert.AreSame(result.Instance.Repository, mock0.Value.Object, "Values didn't match for item 0");
        Assert.AreEqual(typeof(ISampleRepository), mock0.Key, "Key didn't match for item 0");

        Assert.AreSame(result.Instance.Logger, mock1.Value.Object, "Values didn't match for item 1");
        Assert.AreEqual(typeof(ILogger<ClassWithMultipleDependencies>), mock1.Key, "Key didn't match for item 1");
    }

    [TestMethod]
    public void MockCreationResult_GetMockByType()
    {
        var result = MockUtility.CreateInstance<ClassWithMultipleDependencies>();

        Assert.IsNotNull(result);

        Assert.AreEqual(2, result.Mocks.Count, $"Should be mocks created for this instance.");

        var mockOfSlideDataRepository = result.GetMock<ISampleRepository>();
        Assert.IsNotNull(mockOfSlideDataRepository);

        var mockOfLogger = result.GetMock<ILogger<ClassWithMultipleDependencies>>();
        Assert.IsNotNull(mockOfLogger);
    }

    [TestMethod]
    public void MockCreationResult_GetMockByType_BogusDependency()
    {
        var result = MockUtility.CreateInstance<ClassWithDefaultConstructor>();
        Assert.IsNotNull(result);

        var mockOfLogger = result.GetMock<ILogger<ClassWithMultipleDependencies>>();
        Assert.IsNull(mockOfLogger);
    }
}
