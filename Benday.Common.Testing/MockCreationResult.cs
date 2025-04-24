using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Common.Testing;

/// <summary>
/// The result of the creation of an instance of a class with Moq-based mocks.
/// </summary>
/// <typeparam name="T">The type for the class that was created</typeparam>
public class MockCreationResult<T> where T : class
{
    /// <summary>
    /// The instance of the class that was created. This is typically the system under test.
    /// </summary>
    public required T Instance { get; set; }

    /// <summary>
    /// The mocks that were created for the class. This is typically the dependencies of the system under test.
    /// </summary>
    public Dictionary<Type, Mock> Mocks { get; set; } = new();

    /// <summary>
    /// Get a mock of the specified type and throw an exception if it is not found.
    /// </summary>
    /// <typeparam name="TMock">The type that was mocked</typeparam>
    /// <returns>The mocked dependency</returns>
    /// <exception cref="InvalidOperationException">Throws an exception if not found</exception>
    public Mock<TMock> GetRequiredMock<TMock>() where TMock : class
    {
        var mock = GetMock<TMock>() ??
            throw new InvalidOperationException(
                $"Mock of type {typeof(TMock).Name} was not found");

        return mock;
    }

    /// <summary>
    /// Get a mock of the specified type. If not found, null is returned.
    /// </summary>
    /// <typeparam name="TMock">The type that was mocked</typeparam>
    /// <returns>The Moq-based mock instance</returns>
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
