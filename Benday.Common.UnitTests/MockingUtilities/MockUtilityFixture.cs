﻿using Benday.Common.Testing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Linq;
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

        result.Should().NotBeNull();
        result.Mocks.Count.Should().Be(0, "no mocks should be created for this instance");
        result.Instance.Should().NotBeNull("Instance was null");
    }

    [Fact]
    public void CreateInstanceWithMocks_ConstructorTakesNoArgs()
    {
        var result = MockUtility.CreateInstance<ClassWithEmptyConstructor>();

        result.Should().NotBeNull();
        result.Mocks.Count.Should().Be(0, "no mocks should be created for this instance");
        result.Instance.Should().NotBeNull("Instance was null");
    }

    [Fact]
    public void CreateInstanceWithMocks_ConstructorTakesOneInterface()
    {
        var result = MockUtility.CreateInstance<ClassWithOneDependency>();

        result.Should().NotBeNull();
        result.Mocks.Count.Should().Be(1, "a mock should be created for this instance");
        result.Instance.Should().NotBeNull("Instance was null");

        var mock0 = result.Mocks.FirstOrDefault();

        mock0.Should().NotBeNull();
        result.Instance.Repository.Should().BeSameAs(mock0.Value.Object, "Values didn't match");
        mock0.Key.Should().Be(typeof(ISampleRepository), "Key didn't match");
    }

    [Fact]
    public void CreateInstanceWithMocks_ConstructorTakesMultipleParameters()
    {
        var result = MockUtility.CreateInstance<ClassWithMultipleDependencies>();

        result.Should().NotBeNull();
        result.Mocks.Count.Should().Be(2, "mocks should be created for this instance");
        result.Instance.Should().NotBeNull("Instance was null");

        var mock0 = result.Mocks.FirstOrDefault();
        var mock1 = result.Mocks.LastOrDefault();

        mock0.Should().NotBeNull();
        mock1.Should().NotBeNull();

        result.Instance.Repository.Should().BeSameAs(mock0.Value.Object, "Values didn't match for item 0");
        mock0.Key.Should().Be(typeof(ISampleRepository), "Key didn't match for item 0");

        result.Instance.Logger.Should().BeSameAs(mock1.Value.Object, "Values didn't match for item 1");
        mock1.Key.Should().Be(typeof(ILogger<ClassWithMultipleDependencies>), "Key didn't match for item 1");
    }

    [Fact]
    public void MockCreationResult_GetMockByType()
    {
        var result = MockUtility.CreateInstance<ClassWithMultipleDependencies>();

        result.Should().NotBeNull();
        result.Mocks.Count.Should().Be(2, "mocks should be created for this instance");

        var mockOfSlideDataRepository = result.GetMock<ISampleRepository>();
        mockOfSlideDataRepository.Should().NotBeNull();

        var mockOfLogger = result.GetMock<ILogger<ClassWithMultipleDependencies>>();
        mockOfLogger.Should().NotBeNull();
    }

    [Fact]
    public void MockCreationResult_GetMockByType_BogusDependency()
    {
        var result = MockUtility.CreateInstance<ClassWithDefaultConstructor>();

        result.Should().NotBeNull();

        var mockOfLogger = result.GetMock<ILogger<ClassWithMultipleDependencies>>();
        mockOfLogger.Should().BeNull();
    }
}
