using Moq;
using System;
using System.Linq;

namespace PowerpointUtil.Api.Tests.MockingUtilities;

public class MockCreationResult<T> where T : class
{
    public required T Instance { get; set; }

    public Dictionary<Type, Mock> Mocks { get; set; } = new();

    public Mock<TMock> GetRequiredMock<TMock>() where TMock : class
    {
        var mock = GetMock<TMock>() ?? 
            throw new InvalidOperationException(
                $"Mock of type {typeof(TMock).Name} was not found");
                
        return mock;
    }

    public Mock<TMock>? GetMock<TMock>() where TMock : class
    {
        var mock = Mocks.FirstOrDefault(x => x.Key == typeof(TMock));

        if (mock.Value == null)
        {
            return default;
        }
        else
        {
            return (Mock<TMock>)mock.Value;
        }
    }
}
