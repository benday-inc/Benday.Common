using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Moq;

namespace Benday.Common.Testing;

/// <summary>
/// The result of the creation of an instance of a class with Moq-based mocks.
/// </summary>
/// <typeparam name="T">The type for the class that was created</typeparam>
public class MockCreationResult<T> where T : class
{
    public MockCreationResult(
        ConstructorInfo reflectedConstructor,
        object[]? constructorArguments,
        Dictionary<Type, Mock> mocks)
    {
        ConstructorArguments = constructorArguments;
        // Instance = (T)reflectedConstructor.Invoke(constructorArguments);
        ReflectedConstructor = reflectedConstructor;
        Mocks = mocks;
    }

    /// <summary>
    /// The reflected constructor that was used to create the instance.
    /// </summary>
    protected ConstructorInfo ReflectedConstructor { get; }

    protected object[]? ConstructorArguments
    {
        get;
    }

    private T? _instance;

    public bool IsInstanceCreated
    {
        get
        {
            return _instance != null;
        }
    }

    /// <summary>
    /// The instance of the desired class. This is typically the system under test. 
    /// NOTE: This instance is lazy-created at the first access of the Instance property 
    /// so that mocks can be configured before the object creation.
    /// </summary>
    public T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)ReflectedConstructor.Invoke(ConstructorArguments);

                if (_instance == null)
                {
                    throw new InvalidOperationException($"Problem creating instance from constructor.");
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// The mocks that were created for the class. This is typically the dependencies of the system under test.
    /// </summary>
    public Dictionary<Type, Mock> Mocks { get; set; }

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
