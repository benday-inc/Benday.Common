# Benday.Common

A collection of utility classes for .NET including: domain model patterns, search and paging infrastructure, JSON reading/editing, process execution, and extension methods for strings and configuration.

## Key Features

* **Domain Model Support** - Identity interfaces (`IInt32Identity`, `IStringIdentity`) and deletable pattern (`IDeleteable`)
* **Search & Paging** - Classes for handling searching, sorting, and paging of results (`Search`, `SearchArgument`, `SearchResult`, `PageableResults`)
* **JSON Editing** - Path-based JSON reading and editing with `JsonEditor` class (in `Benday.Common.Json` namespace)
* **Process Execution** - Utility classes for running command line tools, reading results, and managing errors (`ProcessRunner`, `AsyncProcessRunner`)
* **String Extensions** - Safe string operations, null handling, and type conversions (`SafeToString`, `SafeToInt32`, `IsNullOrEmpty`, etc.)
* **Configuration Extensions** - Extension methods for accessing values from `IConfiguration`
* **Dependency Injection** - Interfaces for organizing type registration (`ITypeRegistrationItem`)

## Installation

```bash
dotnet add package Benday.Common
```

## Target Frameworks

* .NET 8.0
* .NET 9.0
* .NET 10.0
* .NET Standard 2.1

## Quick Examples

### JSON Editing

```csharp
using Benday.Common.Json;

// Load and edit JSON
var editor = new JsonEditor(jsonString, true);

// Read values at any depth
var value = editor.GetValue("ConnectionStrings", "DefaultConnection");

// Set values (creates intermediate nodes if needed)
editor.SetValue("newValue", "Settings", "Feature", "Enabled");

// Get typed values
bool? isEnabled = editor.GetValueAsBoolean("Settings", "IsEnabled");
int? timeout = editor.GetValueAsInt32("Settings", "Timeout");

// Output modified JSON
string result = editor.ToJson(indented: true);
```

### String Extensions

```csharp
using Benday.Common;

string? maybeNull = GetSomeValue();

// Safe operations that handle nulls
string safe = maybeNull.SafeToString("default");
int number = maybeNull.SafeToInt32(-1);
bool contains = maybeNull.SafeContains("search");

// Null checks as extension methods
if (maybeNull.IsNullOrEmpty()) { /* ... */ }
if (maybeNull.IsNullOrWhitespace()) { /* ... */ }

// Case-insensitive comparison
if (value1.EqualsCaseInsensitive(value2)) { /* ... */ }
```

## About

Written by Benjamin Day
Pluralsight Author | Microsoft MVP | Scrum.org Professional Scrum Trainer
https://www.benday.com
https://www.slidespeaker.ai
info@benday.com
YouTube: https://www.youtube.com/@_benday

## Bugs? Suggestions? Contribute?

*Got ideas for features you'd like to see? Found a bug?
Let us know by submitting an [issue](https://github.com/benday-inc/Benday.Common/issues)*. *Want to contribute? Submit a pull request.*

[Source code](https://github.com/benday-inc/Benday.Common)

[API Documentation](https://benday-inc.github.io/Benday.Common/api/Benday.Common.html)

[NuGet Package](https://www.nuget.org/packages/Benday.Common/)
