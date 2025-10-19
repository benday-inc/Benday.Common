using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Benday.Common.Testing;

/// <summary>
/// Provides static methods for making assertions on collections with descriptive failure messages.
/// </summary>
public static class AssertThatCollection
{
    /// <summary>
    /// Verifies that the collection is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection is not empty.</exception>
    public static void IsEmpty<T>(IEnumerable<T> collection, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (collection.Any())
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "IsEmpty", "Expected empty collection"));
        }
    }

    /// <summary>
    /// Verifies that the collection is not empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection is empty.</exception>
    public static void IsNotEmpty<T>([NotNull] IEnumerable<T> collection, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (!collection.Any())
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "IsNotEmpty", "Expected non-empty collection"));
        }
    }

    /// <summary>
    /// Verifies that the collection has the expected count.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="expectedCount">The expected count.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection does not have the expected count.</exception>
    public static void HasCount<T>(IEnumerable<T> collection, int expectedCount, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        var actualCount = collection.Count();
        if (actualCount != expectedCount)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "HasCount", 
                $"Expected count: {expectedCount}, Actual count: {actualCount}"));
        }
    }

    /// <summary>
    /// Verifies that the collection contains the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to search.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection does not contain the item.</exception>
    public static void Contains<T>(IEnumerable<T> collection, T item, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (!collection.Contains(item))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "Contains", 
                $"Expected to contain: {AssertionMessageFormatter.FormatValue(item)}"));
        }
    }

    /// <summary>
    /// Verifies that the collection does not contain the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to search.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection contains the item.</exception>
    public static void DoesNotContain<T>(IEnumerable<T> collection, T item, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (collection.Contains(item))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "DoesNotContain", 
                $"Expected not to contain: {AssertionMessageFormatter.FormatValue(item)}"));
        }
    }

    /// <summary>
    /// Verifies that all elements in the collection match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="predicate">The predicate to test against each element.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when not all elements match the predicate.</exception>
    public static void AllMatch<T>(IEnumerable<T> collection, Func<T, bool> predicate, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        var failingItems = collection.Where(item => !predicate(item)).ToList();
        if (failingItems.Any())
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "AllMatch", 
                $"Expected all items to match predicate, but {failingItems.Count} items failed"));
        }
    }

    /// <summary>
    /// Verifies that at least one element in the collection matches the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="predicate">The predicate to test against each element.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when no elements match the predicate.</exception>
    public static void AnyMatch<T>(IEnumerable<T> collection, Func<T, bool> predicate, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (!collection.Any(predicate))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "AnyMatch", 
                "Expected at least one item to match predicate, but none did"));
        }
    }

    /// <summary>
    /// Verifies that two collections are equal by comparing their elements in order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="expected">The expected collection.</param>
    /// <param name="actual">The actual collection.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collections are not equal.</exception>
    public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message)
    {
        if (expected == null && actual == null)
        {
            return;
        }

        if (expected == null || actual == null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEqual"));
        }

        var expectedList = expected.ToList();
        var actualList = actual.ToList();

        if (expectedList.Count != actualList.Count)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEqual"));
        }

        for (int i = 0; i < expectedList.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(expectedList[i], actualList[i]))
            {
                throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEqual"));
            }
        }
    }

    /// <summary>
    /// Verifies that the collection is a subset of the superset collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="subset">The collection that should be a subset.</param>
    /// <param name="superset">The collection that should contain all elements of the subset.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the subset is not contained in the superset.</exception>
    public static void IsSubsetOf<T>(IEnumerable<T> subset, IEnumerable<T> superset, string message)
    {
        if (subset == null)
        {
            throw new ArgumentNullException(nameof(subset));
        }

        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        var supersetList = superset.ToList();
        var missingItems = subset.Where(item => !supersetList.Contains(item)).ToList();

        if (missingItems.Any())
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(subset, message, "IsSubsetOf", 
                $"Missing items in superset: {string.Join(", ", missingItems.Select(item => AssertionMessageFormatter.FormatValue(item)))}"));
        }
    }

    /// <summary>
    /// Verifies that the collection is a superset of the subset collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="superset">The collection that should contain all elements of the subset.</param>
    /// <param name="subset">The collection that should be contained in the superset.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the superset does not contain all elements of the subset.</exception>
    public static void IsSupersetOf<T>(IEnumerable<T> superset, IEnumerable<T> subset, string message)
    {
        IsSubsetOf(subset, superset, message);
    }

    /// <summary>
    /// Verifies that the collection contains only unique elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection contains duplicate elements.</exception>
    public static void HasUniqueElements<T>(IEnumerable<T> collection, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        var collectionList = collection.ToList();
        var uniqueElements = collectionList.Distinct().ToList();

        if (collectionList.Count != uniqueElements.Count)
        {
            var duplicates = collectionList.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "HasUniqueElements", 
                $"Found duplicate elements: {string.Join(", ", duplicates.Select(item => AssertionMessageFormatter.FormatValue(item)))}"));
        }
    }
}