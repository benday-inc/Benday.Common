# Benday.Common.Testing

A collection of classes to streamline testing with XUnit and Moq.

## About

Written by Benjamin Day  
Pluralsight Author | Microsoft MVP | Scrum.org Professional Scrum Trainer  
https://www.benday.com  
https://www.slidespeaker.ai  
info@benday.com  
YouTube: https://www.youtube.com/@_benday  

## Key Features

* `TestClassBase` is a base class for XUnit test classes that provides a `WriteLine()` method that hooks into XUnit's 
ITestOutputHelper for writing messages to the console or test results.
* `MockUtility` is a utility class that helps work with Moq-based mocks -- especially when 
testing classes that use dependency injection.
    * Call `CreateInstance<T>()` to create an instance of a class that automatically creates mocks for all constructor parameters
    * Access the generated class and mocks using the `MockCreationResult<T>` instance returned from `CreateInstance<T>()`
    * Organize, access, and verify mocks using the `MockCreationResult<T>` class

## Bugs? Suggestions? Contribute?

*Got ideas for git repo sync features you'd like to see? Found a bug? 
Let us know by submitting an [issue](https://github.com/benday-inc/Benday.Common/issues)*. *Want to contribute? Submit a pull request.*

[Source code](https://github.com/benday-inc/Benday.Common)

[API Documentation](https://benday-inc.github.io/Benday.Common.Testing/api/Benday.Common.Testing.html)

[NuGet Package](https://www.nuget.org/packages/Benday.Common.Testing/)

