using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Benday.Common.Testing;

/// <summary>
/// Provides fluent assertion extension methods for collections.
/// </summary>
public static class CollectionAssertExtensions
{
    /// <summary>
    /// Verifies that the collection is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collection is not empty.</exception>
    public static IEnumerable<T> ShouldBeEmpty<T>(this IEnumerable<T> collection, string message)
    {
        AssertThatCollection.IsEmpty(collection, message);
        return collection;
    }

    /// <summary>
    /// Verifies that the collection is not empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collection is empty.</exception>
    public static IEnumerable<T> ShouldNotBeEmpty<T>([NotNull] this IEnumerable<T> collection, string message)
    {
        AssertThatCollection.IsNotEmpty(collection, message);
        return collection;
    }

    /// <summary>
    /// Verifies that the collection has the expected count.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="expectedCount">The expected count.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collection does not have the expected count.</exception>
    public static IEnumerable<T> ShouldHaveCount<T>(this IEnumerable<T> collection, int expectedCount, string message)
    {
        AssertThatCollection.HasCount(collection, expectedCount, message);
        return collection;
    }

    /// <summary>
    /// Verifies that the collection contains the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to search.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collection does not contain the item.</exception>
    public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> collection, T item, string message)
    {
        AssertThatCollection.Contains(collection, item, message);
        return collection;
    }

    /// <summary>
    /// Verifies that the collection does not contain the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to search.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collection contains the item.</exception>
    public static IEnumerable<T> ShouldNotContain<T>(this IEnumerable<T> collection, T item, string message)
    {
        AssertThatCollection.DoesNotContain(collection, item, message);
        return collection;
    }

    /// <summary>
    /// Verifies that all elements in the collection match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="predicate">The predicate to test against each element.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when not all elements match the predicate.</exception>
    public static IEnumerable<T> ShouldAllMatch<T>(this IEnumerable<T> collection, Func<T, bool> predicate, string message)
    {
        AssertThatCollection.AllMatch(collection, predicate, message);
        return collection;
    }

    /// <summary>
    /// Verifies that at least one element in the collection matches the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="predicate">The predicate to test against each element.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when no elements match the predicate.</exception>
    public static IEnumerable<T> ShouldAnyMatch<T>(this IEnumerable<T> collection, Func<T, bool> predicate, string message)
    {
        AssertThatCollection.AnyMatch(collection, predicate, message);
        return collection;
    }

    /// <summary>
    /// Verifies that the collection is equal to the expected collection by comparing elements in order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="actual">The actual collection.</param>
    /// <param name="expected">The expected collection.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The actual collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collections are not equal.</exception>
    public static IEnumerable<T> ShouldEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message)
    {
        AssertThatCollection.AreEqual(expected, actual, message);
        return actual;
    }

    /// <summary>
    /// Verifies that the collection is a subset of the superset collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="subset">The collection that should be a subset.</param>
    /// <param name="superset">The collection that should contain all elements of the subset.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The subset collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the subset is not contained in the superset.</exception>
    public static IEnumerable<T> ShouldBeSubsetOf<T>(this IEnumerable<T> subset, IEnumerable<T> superset, string message)
    {
        AssertThatCollection.IsSubsetOf(subset, superset, message);
        return subset;
    }

    /// <summary>
    /// Verifies that the collection is a superset of the subset collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="superset">The collection that should contain all elements of the subset.</param>
    /// <param name="subset">The collection that should be contained in the superset.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The superset collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the superset does not contain all elements of the subset.</exception>
    public static IEnumerable<T> ShouldBeSupersetOf<T>(this IEnumerable<T> superset, IEnumerable<T> subset, string message)
    {
        AssertThatCollection.IsSupersetOf(superset, subset, message);
        return superset;
    }

    /// <summary>
    /// Verifies that the collection contains only unique elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The collection for method chaining.</returns>
    /// <exception cref="AssertionException">Thrown when the collection contains duplicate elements.</exception>
    public static IEnumerable<T> ShouldHaveUniqueElements<T>(this IEnumerable<T> collection, string message)
    {
        AssertThatCollection.HasUniqueElements(collection, message);
        return collection;
    }

    /// <summary>
    /// Verifies that the collection contains exactly one element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The single element in the collection.</returns>
    /// <exception cref="AssertionException">Thrown when the collection does not contain exactly one element.</exception>
    public static T ShouldHaveSingleElement<T>(this IEnumerable<T> collection, string message)
    {
        AssertThatCollection.HasCount(collection, 1, message);
        return collection.Single();
    }

    /// <summary>
    /// Verifies that the collection contains exactly one element that matches the predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="predicate">The predicate to test against each element.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <returns>The single matching element in the collection.</returns>
    /// <exception cref="AssertionException">Thrown when the collection does not contain exactly one matching element.</exception>
    public static T ShouldHaveSingleElement<T>(this IEnumerable<T> collection, Func<T, bool> predicate, string message)
    {
        var matches = collection.Where(predicate).ToList();
        
        if (matches.Count == 0)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "ShouldHaveSingleElement",
                "Expected exactly one element matching predicate, but found none"));
        }
        
        if (matches.Count > 1)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "ShouldHaveSingleElement",
                $"Expected exactly one element matching predicate, but found {matches.Count}"));
        }
        
        return matches[0];
    }
}