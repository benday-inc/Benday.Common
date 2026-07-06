# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET multi-project repository containing three published NuGet packages:

- **Benday.Common** (v10.1.3) - A collection of classes for supporting the domain model pattern in .NET Core
- **Benday.Common.Testing** (v3.2.0) - A collection of classes to streamline testing with XUnit and Moq
- **Benday.Common.Interfaces** (v1.0.3) - A small package holding the core interface contracts (identity, repository, service, and multi-tenant abstractions) that the other two libraries build on

Target frameworks vary by project:

- **Benday.Common** - .NET 8.0, .NET 9.0, .NET 10.0, and .NET Standard 2.1
- **Benday.Common.Testing** - .NET 8.0, .NET 9.0, and .NET 10.0
- **Benday.Common.Interfaces** - .NET Standard 2.1

**Note:** There is a solution file (`Benday.Common.slnx`) but no legacy `.sln`. Build and test commands can target either the solution or individual `.csproj` files.

**Repository layout:** Shipping/library projects live under `src/` and unit test projects live under `test/`:
- `src/Benday.Common`, `src/Benday.Common.Interfaces`, `src/Benday.Common.Testing`
- `test/Benday.Common.UnitTests`, `test/Benday.Common.Interfaces.UnitTests`, `test/Benday.Common.Testing.UnitTests`

## Development Commands

### Building
```bash
# Build specific project
dotnet build src/Benday.Common/Benday.Common.csproj
dotnet build src/Benday.Common.Testing/Benday.Common.Testing.csproj
```

### Running Tests
```bash
# Run tests for specific project
dotnet test test/Benday.Common.UnitTests/Benday.Common.UnitTests.csproj
dotnet test test/Benday.Common.Testing.UnitTests/Benday.Common.Testing.UnitTests.csproj

# Run a single test method
dotnet test --filter "MethodName=TestMethodName"
```

Test projects target `net10.0` only.

### Creating NuGet Packages
All three library projects are configured with `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>`, so packages are automatically generated during build.

```bash
dotnet pack src/Benday.Common/Benday.Common.csproj
dotnet pack src/Benday.Common.Testing/Benday.Common.Testing.csproj
dotnet pack src/Benday.Common.Interfaces/Benday.Common.Interfaces.csproj
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
- **Domain Model Support**: Identity interfaces (`IInt32Identity`, `IStringIdentity`) and the selectable pattern (`ISelectable`). The deletable pattern (`IDeleteable`) and other core contracts are defined in **Benday.Common.Interfaces** (see below) and re-used here.
- **Search & Paging**: Search framework with `Search`, `SearchArgument`, `SearchResult`, `SearchMethod`/`SearchOperator` enums, `SearchConstants`, paging with `PageableResults`
- **Sorting & View Models**: `ISortableResult`, `SortableViewModelBase<T>`, `SearchViewModelBase<T>`, `SimpleSearchResults<T>`, `SortBy`
- **Process Execution**: `ProcessRunner`/`IProcessRunner` for synchronous operations, `AsyncProcessRunner`/`IAsyncProcessRunner` for async operations, `ProcessRunnerResult`/`IProcessRunnerResult` for results
- **JSON Utilities** (`Json/` namespace): `JsonEditor` for reading/editing JSON documents, `JsonExtensionMethods` for `JsonElement` and `JsonNode` extension methods (safe getters, array operations, `GetDictionary`), `ElementResult`, `SiblingValueArguments`
- **Extension Methods**: `StringExtensionMethods` (safe conversions, null checks, case-insensitive comparison), `ConfigurationExtensionMethods` (safe config access)
- **Dependency Injection**: `ITypeRegistrationItem`, `TypeRegistrationItem<TService, TImplementation>`

### Benday.Common.Interfaces Library
A small `netstandard2.1`-only package containing just the core interface contracts, so consumers can depend on the abstractions without pulling in the full `Benday.Common` implementation. Both `Benday.Common` and `Benday.Common.Testing` reference it. Contents:
- **Identity & domain contracts**: `IEntityIdentity<TKey>`, `IDeleteable`, `IParentedItem<TKey>`, `IBlobOwner`
- **Multi-tenancy**: `ITenantItem<TKey>`
- **Async repositories**: `IAsyncRepository<T, TKey>`, `IAsyncReadableRepository<T, TKey>`, `IAsyncTenantRepository<T, TKey>`
- **Async services**: `IAsyncService<T, TKey>`, `IAsyncTenantService<T, TKey>`

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
- `AssertThatCollection` - Collection-specific assertions (empty, count, contains, uniqueness, subset/superset, ordered equality via `AreEqual`, unordered/multiset equality via `AreEquivalent`, and per-element assertions via `AllSatisfy`). Elements are compared by value using `EqualityComparer<T>.Default`; overloads accept an `IEqualityComparer<T>` (for custom types / nested collections) or an `Action<T,T>` for custom pairwise comparison
- `AssertThatString` - String-specific assertions (starts/ends with, contains, regex, length, case-insensitive)
- `AssertThatNumeric` - Numeric assertions (comparisons, ranges, approximations, NaN/infinity checks, sign checks)

**Fluent Extension Methods:**
- `ObjectAssertExtensions` - Fluent assertions for all objects (`obj.ShouldEqual(expected, "message")`)
- `CollectionAssertExtensions` - Fluent collection assertions (`list.ShouldHaveCount(5, "message")`, plus `ShouldEqualCollection`, `ShouldBeEquivalentTo`, and `ShouldAllSatisfy`; use `ShouldEqualCollection` instead of `ShouldEqual` on array-typed variables to force element-by-element comparison)
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

- All library projects use nullable reference types (`<Nullable>enable</Nullable>`)
- Code style enforcement is enabled in build (`<EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>`)
- Follow the existing patterns for search functionality, identity interfaces, and dependency injection patterns
- Extension methods are organized in dedicated classes (e.g., `StringExtensionMethods`, `ConfigurationExtensionMethods`, `JsonExtensionMethods`)

## Version Management

- Each library uses semantic versioning, set via the `<TheVersion>` MSBuild property inside its own `.csproj`:
  - Benday.Common - currently `10.1.3`
  - Benday.Common.Testing - currently `3.2.0`
  - Benday.Common.Interfaces - currently `1.0.3`
- All three packages live in this repo and are versioned/released together, so the internal dependencies use `ProjectReference` rather than `PackageReference`: Benday.Common references Benday.Common.Interfaces; Benday.Common.Testing references both Benday.Common and Benday.Common.Interfaces. Because the referenced projects are packable, `dotnet pack` automatically emits the corresponding NuGet dependencies in each `.nupkg` (floored at the version being built). This means the libraries always build from local source (no dependence on a published package), but it also means a referenced package must be published for a dependent package to be installable — publish/release the dependencies together and in order.
