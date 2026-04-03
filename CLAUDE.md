# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET multi-project repository containing two main libraries:

- **Benday.Common** (v9.12.0) - A collection of classes for supporting the domain model pattern in .NET Core
- **Benday.Common.Testing** (v2.2.1) - A collection of classes to streamline testing with XUnit and Moq

Both libraries target .NET 8.0, .NET 9.0, .NET 10.0, and .NET Standard 2.1. They are published as NuGet packages.

**Note:** There is no `.sln` file. Build and test commands must target individual `.csproj` files.

## Development Commands

### Building
```bash
# Build specific project
dotnet build Benday.Common/Benday.Common.csproj
dotnet build Benday.Common.Testing/Benday.Common.Testing.csproj
```

### Running Tests
```bash
# Run tests for specific project
dotnet test Benday.Common.UnitTests/Benday.Common.UnitTests.csproj
dotnet test Benday.Common.Testing.UnitTests/Benday.Common.Testing.UnitTests.csproj

# Run a single test method
dotnet test --filter "MethodName=TestMethodName"
```

Test projects target `net10.0` only.

### Creating NuGet Packages
Both projects are configured with `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>`, so packages are automatically generated during build.

```bash
dotnet pack Benday.Common/Benday.Common.csproj
dotnet pack Benday.Common.Testing/Benday.Common.Testing.csproj
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
- **Domain Model Support**: Identity interfaces (`IInt32Identity`, `IStringIdentity`), deletable pattern (`IDeleteable`), selectable pattern (`ISelectable`)
- **Search & Paging**: Search framework with `Search`, `SearchArgument`, `SearchResult`, `SearchMethod`/`SearchOperator` enums, `SearchConstants`, paging with `PageableResults`
- **Sorting & View Models**: `ISortableResult`, `SortableViewModelBase<T>`, `SearchViewModelBase<T>`, `SimpleSearchResults<T>`, `SortBy`
- **Process Execution**: `ProcessRunner`/`IProcessRunner` for synchronous operations, `AsyncProcessRunner`/`IAsyncProcessRunner` for async operations, `ProcessRunnerResult`/`IProcessRunnerResult` for results
- **JSON Utilities** (`Json/` namespace): `JsonEditor` for reading/editing JSON documents, `JsonExtensionMethods` for `JsonElement` and `JsonNode` extension methods (safe getters, array operations, `GetDictionary`), `ElementResult`, `SiblingValueArguments`
- **Extension Methods**: `StringExtensionMethods` (safe conversions, null checks, case-insensitive comparison), `ConfigurationExtensionMethods` (safe config access)
- **Dependency Injection**: `ITypeRegistrationItem`, `TypeRegistrationItem<TService, TImplementation>`

### Benday.Common.Testing Library
Testing utilities include:
- **TestClassBase**: Base class for XUnit tests with `WriteLine()` method for test output and sample file helpers (`GetSampleFilePath`, `GetSampleFileText`)
- **MockUtility**: Streamlines Moq-based testing with automatic mock creation for constructor dependencies
- **MockCreationResult**: Manages and provides access to created instances and their mocks (supports lazy instantiation for mock configuration before instance creation)
- **Comprehensive Assertion Library**: Enhanced assertions with descriptive failure messages

#### Assertion Library Components
The library includes a comprehensive suite of assertion classes that address XUnit's lack of failure messages:

**Static Assertion Classes:**
- `AssertThat` - Core assertions (equality, null checks, type checks, exceptions, reference equality)
- `AssertThatCollection` - Collection-specific assertions (empty, count, contains, uniqueness, subset/superset)
- `AssertThatString` - String-specific assertions (starts/ends with, contains, regex, length, case-insensitive)
- `AssertThatNumeric` - Numeric assertions (comparisons, ranges, approximations, NaN/infinity checks, sign checks)

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
- Fluent extensions provide the most readable syntax and support method chaining
- All assertions throw `AssertionException` with formatted error messages showing expected vs actual values

## Testing Framework

The project uses:
- **XUnit** for unit testing
- **Moq** for mocking
- Test projects follow the naming convention `*.UnitTests`
- Test projects target `net10.0` only

## Code Conventions

- Both libraries use nullable reference types (`<Nullable>enable</Nullable>`)
- Code style enforcement is enabled in build (`<EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>`)
- Follow the existing patterns for search functionality, identity interfaces, and dependency injection patterns
- Extension methods are organized in dedicated classes (e.g., `StringExtensionMethods`, `ConfigurationExtensionMethods`, `JsonExtensionMethods`)

## Version Management

- Benday.Common uses semantic versioning, currently at `9.12.0`
- Benday.Common.Testing uses semantic versioning, currently at `2.2.1`
- The Testing library depends on Benday.Common version `[9.7.0,)` or higher
