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
    /// If the class has a constructor with parameters, mocks will be created for each parameter type.    /// 
    /// </summary>
    /// <typeparam name="T">Create an instance of this type. This is typically the system under test.</typeparam>
    /// <returns>MockCreationResult that provides access to the instance of T that was instantiated and also access to the Moq mocks.</returns>
    /// <exception cref="InvalidOperationException">Exceptions are thrown if the class does not have any constructors or has multiple constructors.
    /// BTW, if you have suggestions on how to gracefully handle the multi-constructor case, please let me know.
    /// </exception>
    public static MockCreationResult<T> CreateInstance<T>() where T : class
    {
        var constructors = typeof(T).GetConstructors();

        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Type '{typeof(T)}' does not have any constructors.");
        }
        else if (constructors.Length > 1)
        {
            throw new InvalidOperationException($"Type '{typeof(T)}' has multiple constructors.");
        }

        var parameterlessConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);

        if (parameterlessConstructor != null)
        {
            return new MockCreationResult<T>(parameterlessConstructor, null, new Dictionary<Type, Mock>());
        }
        else
        {
            var ctor = constructors[0];
            var parameters = ctor.GetParameters();
            var mocks = new Dictionary<Type, Mock>();

            var args = new List<object>();

            foreach (var parameter in parameters)
            {
                var mockType = typeof(Mock<>).MakeGenericType(parameter.ParameterType);

                var temp = Activator.CreateInstance(mockType);

                if (temp == null)
                {
                    throw new InvalidOperationException($"Failed to create instance of type '{mockType}'.");
                }
                else if (mockType.IsInstanceOfType(temp) == false)
                {
                    throw new InvalidOperationException($"Instance of type '{mockType}' is not a Mock<{parameter.ParameterType}>.");
                }
                else
                {
                    var mock = (Mock)temp;
                    mocks[parameter.ParameterType] = mock;
                    args.Add(mock.Object);
                }
            }

            return new MockCreationResult<T>(ctor, args.ToArray(), mocks);
        }
    }
}
