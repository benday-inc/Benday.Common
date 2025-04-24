using Moq;
using System;
using System.Linq;

namespace PowerpointUtil.Api.Tests.MockingUtilities;

public static class MockUtility
{
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
            var instance = (T)parameterlessConstructor.Invoke(null);

            return new MockCreationResult<T> { Instance = instance };
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

            var instance = (T)ctor.Invoke(args.ToArray());

            return new MockCreationResult<T> { Instance = instance, Mocks = mocks };
        }
    }
}
