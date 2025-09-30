# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET solution containing two main libraries:

- **Benday.Common** - A collection of classes for supporting the domain model pattern in .NET Core
- **Benday.Common.Testing** - A collection of classes to streamline testing with XUnit and Moq

Both libraries target .NET 8.0 and .NET 9.0 frameworks and are published as NuGet packages.

## Development Commands

### Building the Solution
```bash
# Build the entire solution
dotnet build

# Build specific project
dotnet build Benday.Common/Benday.Common.csproj
dotnet build Benday.Common.Testing/Benday.Common.Testing.csproj
```

### Running Tests
```bash
# Run all tests in the solution
dotnet test

# Run tests for specific project
dotnet test Benday.Common.UnitTests/Benday.Common.UnitTests.csproj
dotnet test Benday.Common.Testing.UnitTests/Benday.Common.Testing.UnitTests.csproj

# Run a single test method
dotnet test --filter "MethodName=TestMethodName"
```

### Creating NuGet Packages
Both projects are configured with `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>`, so packages are automatically generated during build.

```bash
# Create packages manually
dotnet pack
```

### Documentation Generation
The project uses DocFX for API documentation:

```bash
# Generate documentation (requires DocFX to be installed)
./generate-docfx-docs.sh

# Serve documentation locally
./serve-docfx-docs.sh

# Copy documentation to docs folder
./copy-docfx-site-to-docs.sh

# Complete documentation workflow
./regenerate-and-copy-docs.ps1
```

## Project Structure

### Benday.Common Library
Core functionality includes:
- **Domain Model Support**: Identity interfaces (`IInt32Identity`, `IStringIdentity`), deletable pattern (`IDeleteable`)
- **Search & Paging**: Search framework with `Search`, `SearchArgument`, `SearchResult`, paging with `PageableResults`
- **Utility Classes**: `ProcessRunner` for command-line operations, extension methods for configuration and strings
- **Dependency Injection**: Type registration interfaces (`ITypeRegistrationItem`)

### Benday.Common.Testing Library
Testing utilities include:
- **TestClassBase**: Base class for XUnit tests with `WriteLine()` method for test output
- **MockUtility**: Streamlines Moq-based testing with automatic mock creation for constructor dependencies
- **MockCreationResult**: Manages and provides access to created instances and their mocks
- **Comprehensive Assertion Library**: Enhanced assertions with descriptive failure messages

#### Assertion Library Components
The library includes a comprehensive suite of assertion classes that address XUnit's lack of failure messages:

**Static Assertion Classes:**
- `AssertThat` - Core assertions (equality, null checks, type checks, exceptions)
- `CollectionAssert` - Collection-specific assertions (empty, count, contains, uniqueness)
- `StringAssert` - String-specific assertions (starts/ends with, contains, regex, length)
- `NumericAssert` - Numeric assertions (comparisons, ranges, approximations, NaN/infinity checks)

**Fluent Extension Methods:**
- `ObjectAssertExtensions` - Fluent assertions for all objects (`obj.ShouldEqual(expected, "message")`)
- `CollectionAssertExtensions` - Fluent collection assertions (`list.ShouldHaveCount(5, "message")`)
- `StringAssertExtensions` - Fluent string assertions (`str.ShouldStartWith("prefix", "message")`)
- `NumericAssertExtensions` - Fluent numeric assertions (`value.ShouldBePositive("message")`)

**Usage Notes:**
- All assertion methods require a descriptive failure message parameter
- Use `AssertThat` for static assertions with descriptive messages:
  ```csharp
  // Use AssertThat for descriptive messages
  AssertThat.AreEqual(expected, actual, "Values should match after transformation");

  // Or use XUnit's Assert for standard assertions
  Xunit.Assert.Equal(expected, actual);

  // Or use fluent syntax (recommended)
  actual.ShouldEqual(expected, "Values should match after transformation");
  ```
- Fluent extensions provide the most readable syntax
- All assertions throw `AssertionException` with formatted error messages showing expected vs actual values

## Testing Framework

The project uses:
- **XUnit** for unit testing
- **Moq** for mocking
- Test projects follow the naming convention `*.UnitTests`

## Code Conventions

- Both libraries use nullable reference types (`<Nullable>enable</Nullable>`)
- Code style enforcement is enabled in build (`<EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>`)
- Follow the existing patterns for search functionality, identity interfaces, and dependency injection patterns
- Extension methods are organized in dedicated classes (e.g., `StringExtensionMethods`, `ConfigurationExtensionMethods`)

## Version Management

- Benday.Common uses semantic versioning in the format `9.x.x`
- Benday.Common.Testing uses semantic versioning in the format `1.x.x`
- The Testing library depends on Benday.Common version `[9.4,)` or higher