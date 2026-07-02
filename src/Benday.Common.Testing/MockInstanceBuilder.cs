using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Moq;

namespace Benday.Common.Testing;

/// <summary>
/// A fluent builder for creating instances of classes with a mix of
/// Moq-based mocks and explicit values for constructor parameters.
/// Use <c>MockUtility.Build&lt;T&gt;()</c> to create an instance of this builder.
/// </summary>
/// <typeparam name="T">The type to create. This is typically the system under test.</typeparam>
public class MockInstanceBuilder<T> where T : class
{
    private Type[]? _constructorParameterTypes;
    private readonly Dictionary<Type, Queue<object>> _positionalValues = new();

    /// <summary>
    /// Selects which constructor to use by specifying the parameter types.
    /// Required when the target type has multiple constructors.
    /// </summary>
    /// <param name="parameterTypes">The types of the constructor parameters, in order.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MockInstanceBuilder<T> UsingConstructor(params Type[] parameterTypes)
    {
        _constructorParameterTypes = parameterTypes;
        return this;
    }

    /// <summary>
    /// Provides an explicit value for a constructor parameter by type.
    /// Use this for non-mockable types like strings, primitives, and enums.
    /// Values are assigned positionally: the first call to WithValue for a given type
    /// is assigned to the first constructor parameter of that type, the second call
    /// to the second parameter of that type, and so on.
    /// Parameters not provided via WithValue will be auto-mocked if they are interfaces.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, matching the constructor parameter type.</typeparam>
    /// <param name="value">The value to pass for this parameter.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public MockInstanceBuilder<T> WithValue<TValue>(TValue value)
    {
        var type = typeof(TValue);

        if (!_positionalValues.TryGetValue(type, out var queue))
        {
            queue = new Queue<object>();
            _positionalValues[type] = queue;
        }

        queue.Enqueue(value!);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockCreationResult{T}"/> by resolving the constructor,
    /// auto-mocking interface parameters, and applying any explicit values.
    /// </summary>
    /// <returns>A MockCreationResult containing the lazily-created instance and its mocks.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no constructors exist, multiple constructors exist without UsingConstructor,
    /// the specified constructor is not found, or a non-mockable parameter has no value provided.
    /// </exception>
    public MockCreationResult<T> Build()
    {
        var constructor = ResolveConstructor();
        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            return new MockCreationResult<T>(constructor, null, new Dictionary<Type, Mock>());
        }

        var mocks = new Dictionary<Type, Mock>();
        var args = new List<object>();

        foreach (var parameter in parameters)
        {
            if (_positionalValues.TryGetValue(parameter.ParameterType, out var queue) &&
                queue.Count > 0)
            {
                args.Add(queue.Dequeue());
            }
            else if (parameter.ParameterType.IsInterface)
            {
                var mock = CreateMock(parameter.ParameterType);
                mocks[parameter.ParameterType] = mock;
                args.Add(mock.Object);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Parameter '{parameter.Name}' of type '{parameter.ParameterType.Name}' " +
                    $"is not an interface and cannot be auto-mocked. " +
                    $"Use WithValue<{parameter.ParameterType.Name}>(value) to provide a value.");
            }
        }

        return new MockCreationResult<T>(constructor, args.ToArray(), mocks);
    }

    private ConstructorInfo ResolveConstructor()
    {
        var constructors = typeof(T).GetConstructors();

        if (constructors.Length == 0)
        {
            throw new InvalidOperationException(
                $"Type '{typeof(T).Name}' does not have any public constructors.");
        }

        if (_constructorParameterTypes != null)
        {
            var match = constructors.FirstOrDefault(c =>
            {
                var ctorParams = c.GetParameters();

                if (ctorParams.Length != _constructorParameterTypes.Length)
                {
                    return false;
                }

                for (var i = 0; i < ctorParams.Length; i++)
                {
                    if (ctorParams[i].ParameterType != _constructorParameterTypes[i])
                    {
                        return false;
                    }
                }

                return true;
            });

            if (match == null)
            {
                var typeNames = string.Join(", ",
                    _constructorParameterTypes.Select(t => t.Name));

                throw new InvalidOperationException(
                    $"Type '{typeof(T).Name}' does not have a constructor " +
                    $"with parameter types ({typeNames}).");
            }

            return match;
        }

        if (constructors.Length > 1)
        {
            throw new InvalidOperationException(
                $"Type '{typeof(T).Name}' has multiple constructors. " +
                $"Use UsingConstructor() to specify which constructor to use.");
        }

        return constructors[0];
    }

    private static Mock CreateMock(Type interfaceType)
    {
        var mockType = typeof(Mock<>).MakeGenericType(interfaceType);

        var temp = Activator.CreateInstance(mockType);

        if (temp == null)
        {
            throw new InvalidOperationException(
                $"Failed to create mock of type '{interfaceType.Name}'.");
        }

        return (Mock)temp;
    }
}
