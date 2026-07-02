using System.Reflection;

using Benday.Common.Testing;

using Microsoft.Extensions.Logging;

using Xunit;

namespace Benday.Common.UnitTests.MockingUtilities;

public class MockInstanceBuilderFixture : TestClassBase
{
    public MockInstanceBuilderFixture(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Build_SingleConstructor_AllInterfaces_NoConfiguration()
    {
        // should work just like CreateInstance<T>() for simple cases
        var result = MockUtility.Build<ClassWithOneDependency>().Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "a mock should be created for this instance");
        result.Instance.ShouldNotBeNull("Instance was null");
        result.Instance.Repository.ShouldBeSameAs(
            result.Mocks[typeof(ISampleRepository)].Object,
            "Repository mock should match");
    }

    [Fact]
    public void Build_SingleConstructor_DefaultConstructor()
    {
        var result = MockUtility.Build<ClassWithDefaultConstructor>().Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(0, "no mocks should be created");
        result.Instance.ShouldNotBeNull("Instance was null");
    }

    [Fact]
    public void Build_SingleConstructor_MultipleInterfaces()
    {
        var result = MockUtility.Build<ClassWithMultipleDependencies>().Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(2, "two mocks should be created");
        result.Instance.ShouldNotBeNull("Instance was null");

        result.GetMock<ISampleRepository>().ShouldNotBeNull(
            "ISampleRepository mock should exist");
        result.GetMock<ILogger<ClassWithMultipleDependencies>>().ShouldNotBeNull(
            "ILogger mock should exist");
    }

    [Fact]
    public void Build_MultipleConstructors_ThrowsWithoutUsingConstructor()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            MockUtility.Build<ClassWithMultipleConstructors>().Build();
        });

        ex.Message.ShouldContain("multiple constructors",
            "Exception should mention multiple constructors");
        ex.Message.ShouldContain("UsingConstructor",
            "Exception should mention UsingConstructor");
    }

    [Fact]
    public void Build_MultipleConstructors_SelectsCorrectConstructor()
    {
        var result = MockUtility.Build<ClassWithMultipleConstructors>()
            .UsingConstructor(typeof(string), typeof(ISampleRepository))
            .WithValue("test-value")
            .Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "one mock should be created for ISampleRepository");
        result.Instance.ShouldNotBeNull("Instance was null");
        result.Instance.Name.ShouldEqual("test-value", "Name should match provided value");
        result.Instance.Repository.ShouldNotBeNull("Repository should be mocked");
    }

    [Fact]
    public void Build_MultipleConstructors_SelectsSmallerConstructor()
    {
        var result = MockUtility.Build<ClassWithMultipleConstructors>()
            .UsingConstructor(typeof(string))
            .WithValue("just-a-name")
            .Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(0, "no mocks needed for string-only constructor");
        result.Instance.ShouldNotBeNull("Instance was null");
        result.Instance.Name.ShouldEqual("just-a-name", "Name should match");
        result.Instance.Repository.ShouldBeNull("Repository should be null");
    }

    [Fact]
    public void Build_MixedParameters_StringAndIntWithInterface()
    {
        var result = MockUtility.Build<ClassWithMixedParameters>()
            .WithValue("hello")
            .WithValue(42)
            .Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "one mock for ISampleRepository");
        result.Instance.ShouldNotBeNull("Instance was null");
        result.Instance.Name.ShouldEqual("hello", "Name should match");
        result.Instance.Count.ShouldEqual(42, "Count should match");
        result.Instance.Repository.ShouldNotBeNull("Repository should be mocked");
    }

    [Fact]
    public void Build_NonMockableParameter_ThrowsWithoutWithValue()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            MockUtility.Build<ClassWithMixedParameters>().Build();
        });

        ex.Message.ShouldContain("name", "Exception should mention parameter name");
        ex.Message.ShouldContain("WithValue", "Exception should mention WithValue");
    }

    [Fact]
    public void Build_InvalidConstructorSignature_Throws()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            MockUtility.Build<ClassWithMultipleConstructors>()
                .UsingConstructor(typeof(int), typeof(bool))
                .Build();
        });

        ex.Message.ShouldContain("does not have a constructor",
            "Exception should explain the constructor was not found");
    }

    [Fact]
    public void Build_LazyInstantiation_AllowsMockConfiguration()
    {
        var result = MockUtility.Build<ClassWithOneDependencyAndValidationLogicInTheConstructor>()
            .Build();

        result.IsInstanceCreated.ShouldBeFalse("Instance should not be created yet");

        var mock = result.GetRequiredMock<ISampleConfigurationInfo>();
        mock.SetupGet(x => x.ConfigurationValue).Returns("Configured Value");

        result.Instance.ShouldNotBeNull("Instance was null after mock configuration");
        result.IsInstanceCreated.ShouldBeTrue("Instance should be created now");
    }

    [Fact]
    public void Build_PositionalValues_TwoStringsAssignedInOrder()
    {
        var result = MockUtility.Build<ClassWithTwoStringsAndInterface>()
            .WithValue("Ben")
            .WithValue("Day")
            .WithValue(99)
            .Build();

        result.ShouldNotBeNull("Result should not be null");
        result.Mocks.Count.ShouldEqual(1, "one mock for ISampleRepository");
        result.Instance.ShouldNotBeNull("Instance was null");
        result.Instance.FirstName.ShouldEqual("Ben", "First string should go to firstName");
        result.Instance.LastName.ShouldEqual("Day", "Second string should go to lastName");
        result.Instance.Age.ShouldEqual(99, "Int value should go to age");
        result.Instance.Repository.ShouldNotBeNull("Repository should be mocked");
    }

    [Fact]
    public void Build_GetMockByType_WorksWithBuilder()
    {
        var result = MockUtility.Build<ClassWithMultipleConstructors>()
            .UsingConstructor(typeof(string), typeof(ISampleRepository))
            .WithValue("test")
            .Build();

        var repoMock = result.GetMock<ISampleRepository>();
        repoMock.ShouldNotBeNull("Should be able to retrieve mock by type");

        var bogus = result.GetMock<ILogger<ClassWithMultipleConstructors>>();
        bogus.ShouldBeNull("Should return null for non-existent mock");
    }
}
