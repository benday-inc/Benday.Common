using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Common.Testing;

/// <summary>
/// This class is used to create instances of classes using reflection and Moq.
/// </summary>
public static class MockUtility
{
    /// <summary>
    /// Creates an instance of a class using reflection and Moq.
    /// If the class has a parameterless constructor, it will be used.
    /// If the class has a constructor with parameters, a mock will be created for each parameter.
    /// Only interfaces and abstract classes are auto-mocked.
    /// </summary>
    /// <typeparam name="T">Create an instance of this type. This is typically the system under test.</typeparam>
    /// <returns>MockCreationResult that provides access to the instance of T that was instantiated and also access to the Moq mocks.</returns>
    /// <exception cref="InvalidOperationException">Exceptions are thrown if the class does not have any public constructors,
    /// has multiple constructors, or has a parameter that is not an interface or abstract class.
    /// For those cases, use <c>MockUtility.Build&lt;T&gt;()</c> instead.
    /// </exception>
    public static MockCreationResult<T> CreateInstance<T>() where T : class
    {
        return new MockInstanceBuilder<T>().Build();
    }

    /// <summary>
    /// Creates a fluent builder for constructing an instance of a class with
    /// a mix of Moq-based mocks and explicit values for constructor parameters.
    /// Use this when the class has multiple constructors or has non-mockable parameters
    /// like strings, primitives, or enums.
    /// </summary>
    /// <typeparam name="T">The type to create. This is typically the system under test.</typeparam>
    /// <returns>A MockInstanceBuilder for fluent configuration.</returns>
    public static MockInstanceBuilder<T> Build<T>() where T : class
    {
        return new MockInstanceBuilder<T>();
    }
}
