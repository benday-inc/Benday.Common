# Benday.Common.Testing

A comprehensive testing library for .NET that streamlines unit testing with XUnit and Moq, featuring a powerful assertion framework with fluent syntax and descriptive error messages.

## About

Written by Benjamin Day
Pluralsight Author | Microsoft MVP | Scrum.org Professional Scrum Trainer
https://www.benday.com
https://www.slidespeaker.ai
info@benday.com
YouTube: https://www.youtube.com/@_benday

## Key Features

### 1. Comprehensive Assertion Framework
The library provides a complete assertion framework that addresses XUnit's lack of descriptive failure messages. All assertions support optional custom messages for better debugging.

#### Static Assertion Classes
* **`AssertThat`** - Core assertions for general testing
  * Equality checks (`AreEqual`, `AreNotEqual`)
  * Null checks (`IsNull`, `IsNotNull`)
  * Boolean assertions (`IsTrue`, `IsFalse`)
  * Type checks (`IsOfType`, `IsNotOfType`)
  * Reference equality (`AreSame`, `AreNotSame`)
  * Exception testing (`Throws`, `DoesNotThrow`)

* **`AssertThatString`** - String-specific assertions
  * Null/empty checks (`IsNullOrEmpty`, `IsNotNullOrEmpty`, `IsNullOrWhiteSpace`)
  * String comparison (`StartsWith`, `EndsWith`, `Contains`)
  * Pattern matching (`Matches` with regex support)
  * Length validation (`HasLength`)
  * Case-insensitive comparisons

* **`AssertThatCollection`** - Collection assertions
  * Empty checks (`IsEmpty`, `IsNotEmpty`)
  * Count validation (`HasCount`)
  * Element presence (`Contains`, `DoesNotContain`)
  * Subset/superset validation
  * Uniqueness checks (`HasUniqueElements`)
  * Element matching (`AllMatch`, `AnyMatch`)

* **`AssertThatNumeric`** - Numeric assertions
  * Comparison operators (`IsGreaterThan`, `IsLessThan`, etc.)
  * Range validation (`IsInRange`, `IsNotInRange`)
  * Sign checks (`IsPositive`, `IsNegative`, `IsZero`)
  * Approximate equality for floating-point numbers
  * Special value checks (`IsNotNaN`, `IsFinite`)

#### Fluent Extension Methods
For more readable tests, use the fluent extension methods:

```csharp
// Object assertions
actual.ShouldEqual(expected);
actual.ShouldNotBeNull();
actual.ShouldBeOfType<string>();

// String assertions
myString.ShouldNotBeNullOrEmpty()
        .ShouldStartWith("Hello")
        .ShouldContain("World");

// Collection assertions
myList.ShouldHaveCount(5)
      .ShouldContain(expectedItem)
      .ShouldHaveUniqueElements();

// Numeric assertions
myValue.ShouldBePositive()
       .ShouldBeGreaterThan(0)
       .ShouldBeLessThan(100);
```

#### Simplified Syntax (New!)
All assertion methods now support optional message parameters. You can write simpler assertions without sacrificing error message quality:

```csharp
// Traditional syntax with custom message
AssertThat.AreEqual(expected, actual, "Values should match after transformation");

// Simplified syntax - still produces descriptive error messages
AssertThat.AreEqual(expected, actual);

// Fluent syntax (recommended)
actual.ShouldEqual(expected);
```

### 2. Test Base Class
* **`TestClassBase`** - Base class for XUnit test classes
  * Provides `WriteLine()` method that integrates with XUnit's ITestOutputHelper
  * Simplifies console output in tests

### 3. Mocking Utilities
* **`MockUtility`** - Streamlines Moq-based testing for dependency injection scenarios
  * `CreateInstance<T>()` - Automatically creates mocks for all constructor parameters
  * Returns `MockCreationResult<T>` for easy mock management

* **`MockCreationResult<T>`** - Manages created instances and their mocks
  * Access the created instance and all generated mocks
  * Organize and verify mock interactions

## Installation

Install via NuGet Package Manager:

```bash
Install-Package Benday.Common.Testing
```

Or via .NET CLI:

```bash
dotnet add package Benday.Common.Testing
```

## Usage Examples

### Using the Assertion Framework

```csharp
public class CalculatorTests : TestClassBase
{
    public CalculatorTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        var result = calculator.Add(2, 3);

        // Assert using static methods
        AssertThat.AreEqual(5, result);

        // Or use fluent syntax (recommended)
        result.ShouldEqual(5)
              .ShouldBePositive()
              .ShouldBeInRange(1, 10);
    }

    [Fact]
    public void Divide_ByZero_ThrowsException()
    {
        // Arrange
        var calculator = new Calculator();
        Action divideByZero = () => calculator.Divide(10, 0);

        // Assert
        divideByZero.ShouldThrow<DivideByZeroException>();
    }
}
```

### Using MockUtility for Dependency Injection

```csharp
public class ServiceTests : TestClassBase
{
    public ServiceTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ProcessOrder_ValidOrder_CallsRepository()
    {
        // Arrange
        var mockUtility = new MockUtility();
        var mockResult = mockUtility.CreateInstance<OrderService>();
        var service = mockResult.Instance;

        var order = new Order { Id = 1, Total = 100.00m };

        // Setup mock behavior
        mockResult.GetMock<IOrderRepository>()
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(true);

        // Act
        var result = service.ProcessOrder(order);

        // Assert
        result.ShouldBeTrue();

        // Verify mock was called
        mockResult.GetMock<IOrderRepository>()
            .Verify(x => x.Save(order), Times.Once);
    }
}
```

### Chaining Assertions for Comprehensive Tests

```csharp
[Fact]
public void ValidateUserData_CompleteProfile_PassesAllValidations()
{
    // Arrange
    var userData = GetUserData();

    // Act & Assert with fluent chaining
    userData.Name
        .ShouldNotBeNullOrEmpty()
        .ShouldStartWith("John")
        .ShouldHaveLength(8);

    userData.Email
        .ShouldMatch(@"^[\w\.-]+@[\w\.-]+\.\w+$")
        .ShouldContain("@");

    userData.Permissions
        .ShouldNotBeEmpty()
        .ShouldHaveCount(3)
        .ShouldContain("read")
        .ShouldHaveUniqueElements();

    userData.Age
        .ShouldBePositive()
        .ShouldBeInRange(18, 120);
}
```

## Why Use Benday.Common.Testing?

1. **Better Error Messages**: Unlike standard XUnit assertions, our framework provides descriptive error messages that include expected and actual values, making test failures easier to diagnose.

2. **Fluent Syntax**: Chain multiple assertions for more readable and maintainable tests.

3. **Comprehensive Coverage**: Specialized assertions for strings, collections, numerics, and more.

4. **Simplified Mocking**: MockUtility eliminates boilerplate code when testing classes with dependency injection.

5. **Optional Messages**: New overloads allow simpler syntax while maintaining informative error messages.

## Requirements

- .NET 8.0 or .NET 9.0
- XUnit 2.9.2 or higher
- Moq 4.20.72 or higher

## Bugs? Suggestions? Contribute?

*Got ideas for features you'd like to see? Found a bug?
Let us know by submitting an [issue](https://github.com/benday-inc/Benday.Common/issues)*. *Want to contribute? Submit a pull request.*

[Source code](https://github.com/benday-inc/Benday.Common)

[API Documentation](https://benday-inc.github.io/Benday.Common/api/Benday.Common.Testing.html)

[NuGet Package](https://www.nuget.org/packages/Benday.Common.Testing/)

