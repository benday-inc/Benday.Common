# Benday.Common & Benday.Common.Testing

**Benday.Common** is a collection of classes for supporting the domain model pattern, searching, paging, and utilities in .NET.
**Benday.Common.Testing** is a collection of classes to streamline testing with XUnit and Moq, including a comprehensive assertion library with descriptive failure messages.

## Installation

Install via NuGet Package Manager:

```bash
# For domain model and utility classes
dotnet add package Benday.Common

# For testing utilities and enhanced assertions
dotnet add package Benday.Common.Testing
```

Or via Package Manager Console:
```powershell
Install-Package Benday.Common
Install-Package Benday.Common.Testing
```

## Quick Start: Benday.Common

### Search and Paging
```csharp
// Create a search with arguments
var search = new Search();
search.Arguments.Add(new SearchArgument("name", SearchOperator.Contains, "John"));
search.Arguments.Add(new SearchArgument("isActive", SearchOperator.Equals, "true"));
search.Sorts.Add(new SortBy("lastName", SortDirection.Ascending));
search.MaxNumberOfResults = 50;

// Use in your repository or service layer
var results = userRepository.Search(search);
```

### Identity Interfaces
```csharp
// Implement standard identity patterns
public class User : IInt32Identity, IDeleteable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsMarkedForDelete { get; set; }
}
```

### Process Runner
```csharp
// Run command line tools and capture output
var startInfo = new ProcessStartInfo("git", "status --porcelain");
var runner = new ProcessRunner(startInfo);
var result = runner.Run();

if (result.ExitCode == 0)
{
    Console.WriteLine($"Git status: {result.StandardOutput}");
}
else
{
    Console.WriteLine($"Error: {result.StandardError}");
}
```

### String Extensions
```csharp
// Safe string operations
string value = null;
string safe = value.SafeToString("default"); // Returns "default"
string result = value.ToStringThrowIfNullOrEmpty(); // Throws exception

// Null/empty checks with compiler hints
if (value.IsNullOrEmpty())
{
    // Compiler knows value is null or empty
}
```

## Quick Start: Benday.Common.Testing

### Test Base Class
```csharp
public class MyTestClass : TestClassBase
{
    public MyTestClass(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void MyTest()
    {
        WriteLine("This message appears in test output");
        
        // Load test data from sample files
        var testData = GetSampleFileText("test-data.json");
    }
}
```

### Mock Utility
```csharp
[Fact]
public void TestServiceWithDependencies()
{
    // Automatically create mocks for all constructor dependencies
    var mockResult = MockUtility.CreateInstance<MyService>();
    
    var service = mockResult.Instance;
    var mockRepo = mockResult.GetMock<IRepository>();
    var mockLogger = mockResult.GetMock<ILogger>();
    
    // Configure mocks
    mockRepo.Setup(x => x.GetUser(123)).Returns(new User { Id = 123 });
    
    // Test your service
    var result = service.GetUser(123);
    
    // Verify
    mockRepo.Verify(x => x.GetUser(123), Times.Once);
}
```

### Enhanced Assertions with Descriptive Messages
```csharp
[Fact]
public void TestWithDescriptiveAssertions()
{
    var user = userService.CreateUser("John", "Doe");
    
    // Fluent assertions with descriptive failure messages
    user.ShouldNotBeNull("User should be created successfully");
    user.FirstName.ShouldEqual("John", "First name should be set correctly");
    user.Email.ShouldNotBeNullOrEmpty("Email should be generated for new user");
    
    var users = userService.GetActiveUsers();
    users.ShouldNotBeEmpty("Should have at least one active user");
    users.ShouldContain(user, "New user should appear in active users list");
    
    // Numeric assertions
    user.Id.ShouldBeGreaterThan(0, "User ID should be assigned");
    
    // String assertions
    user.Email.ShouldEndWith("@company.com", "Email should use company domain");
    user.FullName.ShouldMatch(@"^[A-Z][a-z]+ [A-Z][a-z]+$", "Full name should be properly formatted");
}
```

## Key Features: Benday.Common

* **ProcessRunner** - Execute command line tools and capture output with error handling
* **Search Framework** - Flexible search system with arguments, operators, and sorting
* **Paging Support** - `PageableResults<T>` for handling large result sets
* **Identity Interfaces** - Standard patterns for object identity (`IInt32Identity`, `IStringIdentity`)
* **String Extensions** - Safe string operations with null handling and compiler hints
* **Configuration Extensions** - Enhanced IConfiguration access with null safety
* **Type Registration** - Interfaces for organizing dependency injection registration

## Key Features: Benday.Common.Testing

* **TestClassBase** - Base class for XUnit tests with integrated test output
* **MockUtility** - Automatic mock creation for dependencies in constructor injection
* **MockCreationResult** - Organized access to instances and their mocks
* **Comprehensive Assertion Library** - Enhanced assertions that solve XUnit's lack of failure messages:
  * **Static Assertions** - `Assert`, `CollectionAssert`, `StringAssert`, `NumericAssert`
  * **Fluent Extensions** - Chain-able assertions like `value.ShouldEqual(expected, "message")`
  * **Rich Error Messages** - Detailed failure information with expected vs actual formatting
  * **Type Safety** - Full nullable reference type support

## Advanced Usage

### Custom Search Operators
```csharp
// Extend search functionality
public class CustomSearchOperator : SearchOperator
{
    public const string IsWithinDateRange = "IsWithinDateRange";
    public const string ContainsAnyOf = "ContainsAnyOf";
}

// Use in searches
search.Arguments.Add(new SearchArgument("createdDate", CustomSearchOperator.IsWithinDateRange, "2024-01-01|2024-12-31"));
```

### Paging Large Results
```csharp
// Implement paging in your service
public PageableResults<User> GetUsers(int pageNumber = 1, int pageSize = 25)
{
    var search = new Search();
    search.StartIndex = (pageNumber - 1) * pageSize;
    search.MaxNumberOfResults = pageSize;
    
    var results = repository.Search(search);
    
    return new PageableResults<User>
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalCount = repository.GetTotalCount(),
        Results = results
    };
}
```

### Assertion Library Troubleshooting

**Problem: Assert class conflicts with XUnit.Assert**
```csharp
// Solution 1: Use fully qualified names
Benday.Common.Testing.Assert.AreEqual(expected, actual, "Values should match");
Xunit.Assert.Equal(expected, actual); // XUnit's standard assertion

// Solution 2: Use fluent extensions (recommended)
actual.ShouldEqual(expected, "Values should match");

// Solution 3: Alias the namespaces
using BendayAssert = Benday.Common.Testing.Assert;
using XunitAssert = Xunit.Assert;
```

**Problem: Need to test async methods**
```csharp
[Fact]
public async Task TestAsyncMethod()
{
    var service = MockUtility.CreateInstance<AsyncService>();
    
    var result = await service.Instance.GetDataAsync();
    
    result.ShouldNotBeNull("Async method should return data");
    result.Items.ShouldHaveCount(3, "Should return expected number of items");
}
```

## Migration from XUnit Assertions

Replace XUnit assertions with descriptive Benday assertions:

```csharp
// Before (XUnit)
Assert.Equal(expected, actual);
Assert.True(condition);
Assert.NotNull(value);
Assert.Contains(item, collection);

// After (Benday fluent)
actual.ShouldEqual(expected, "Describe why these should be equal");
condition.ShouldBeTrue("Describe what condition should be true");
value.ShouldNotBeNull("Describe why this shouldn't be null");
collection.ShouldContain(item, "Describe why collection should contain this item");
```

## About

Written by Benjamin Day  
Pluralsight Author | Microsoft MVP | Scrum.org Professional Scrum Trainer  
https://www.benday.com  
https://www.slidespeaker.ai  
info@benday.com  
YouTube: https://www.youtube.com/@_benday  

## Links & Resources

[Source code](https://github.com/benday-inc/Benday.Common)

[API Documentation](https://benday-inc.github.io/Benday.Common/api/Benday.Common.html)

[Benday.Common NuGet Package](https://www.nuget.org/packages/Benday.Common/)

[Benday.Common.Testing NuGet Package](https://www.nuget.org/packages/Benday.Common.Testing/)

## Bugs? Suggestions? Contribute?

Got ideas for features you'd like to see? Found a bug? 
Let us know by submitting an [issue](https://github.com/benday-inc/Benday.Common/issues). Want to contribute? Submit a pull request.

