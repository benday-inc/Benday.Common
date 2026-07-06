using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Benday.Common.Testing;

/// <summary>
/// Provides static methods for making assertions on collections with descriptive failure messages.
/// </summary>
/// <remarks>
/// <para>
/// <b>How elements are compared:</b> unless an overload that accepts an
/// <see cref="IEqualityComparer{T}"/> or a custom comparison <see cref="Action{T, T}"/> is used,
/// every method compares elements by <b>value</b> using <see cref="EqualityComparer{T}.Default"/>.
/// For element types that implement <see cref="IEquatable{T}"/> or override
/// <see cref="object.Equals(object)"/> (for example <see cref="string"/>, <see cref="int"/>,
/// <see cref="DateTime"/>, and value types generally) this is value equality.
/// For reference types that do <i>not</i> override equality (most custom classes) and for
/// nested collections (for example <c>string[]</c> as an element of a <c>List&lt;string[]&gt;</c>),
/// <see cref="EqualityComparer{T}.Default"/> falls back to <b>reference</b> equality. In those
/// cases pass an <see cref="IEqualityComparer{T}"/> or use a comparison-<see cref="Action{T, T}"/>
/// overload so the comparison is done the way you intend.
/// </para>
/// <para>
/// <b>Order:</b> <see cref="AreEqual{T}(IEnumerable{T}, IEnumerable{T}, string)"/> compares
/// elements <i>in order</i>. To compare contents while ignoring order (multiset / bag equality),
/// use <see cref="AreEquivalent{T}(IEnumerable{T}, IEnumerable{T}, string)"/>.
/// </para>
/// </remarks>
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
    /// <remarks>
    /// Membership is determined by value using <see cref="EqualityComparer{T}.Default"/> (see the
    /// class-level remarks). Use
    /// <see cref="Contains{T}(IEnumerable{T}, T, IEqualityComparer{T}, string)"/> to supply your own
    /// equality.
    /// </remarks>
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
    /// <remarks>
    /// Membership is determined by value using <see cref="EqualityComparer{T}.Default"/> (see the
    /// class-level remarks). Use
    /// <see cref="DoesNotContain{T}(IEnumerable{T}, T, IEqualityComparer{T}, string)"/> to supply
    /// your own equality.
    /// </remarks>
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
    /// Verifies that two collections are equal by comparing their elements <b>in order</b>.
    /// </summary>
    /// <remarks>
    /// Elements are compared by value using <see cref="EqualityComparer{T}.Default"/>. See the
    /// class-level remarks for exactly what that means and when it falls back to reference equality.
    /// To supply your own equality, use
    /// <see cref="AreEqual{T}(IEnumerable{T}, IEnumerable{T}, IEqualityComparer{T}, string)"/> or
    /// <see cref="AreEqual{T}(IEnumerable{T}, IEnumerable{T}, Action{T, T}, string)"/>. To ignore
    /// order, use <see cref="AreEquivalent{T}(IEnumerable{T}, IEnumerable{T}, string)"/>.
    /// </remarks>
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
    /// <remarks>
    /// Membership is determined by value using <see cref="EqualityComparer{T}.Default"/> (see the
    /// class-level remarks). Use
    /// <see cref="IsSubsetOf{T}(IEnumerable{T}, IEnumerable{T}, IEqualityComparer{T}, string)"/> to
    /// supply your own equality.
    /// </remarks>
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
    /// <remarks>
    /// Membership is determined by value using <see cref="EqualityComparer{T}.Default"/> (see the
    /// class-level remarks). Use
    /// <see cref="IsSupersetOf{T}(IEnumerable{T}, IEnumerable{T}, IEqualityComparer{T}, string)"/> to
    /// supply your own equality.
    /// </remarks>
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
    /// <remarks>
    /// Uniqueness is determined by value using <see cref="EqualityComparer{T}.Default"/> (see the
    /// class-level remarks). Use
    /// <see cref="HasUniqueElements{T}(IEnumerable{T}, IEqualityComparer{T}, string)"/> to supply
    /// your own equality.
    /// </remarks>
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

    /// <summary>
    /// Verifies that the collection contains the specified item, using the supplied comparer to
    /// determine equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to search.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="comparer">The comparer used to determine whether an element matches <paramref name="item"/>.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection does not contain the item.</exception>
    public static void Contains<T>(IEnumerable<T> collection, T item, IEqualityComparer<T> comparer, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        if (!collection.Contains(item, comparer))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "Contains",
                $"Expected to contain: {AssertionMessageFormatter.FormatValue(item)}"));
        }
    }

    /// <summary>
    /// Verifies that the collection does not contain the specified item, using the supplied comparer
    /// to determine equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to search.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="comparer">The comparer used to determine whether an element matches <paramref name="item"/>.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection contains the item.</exception>
    public static void DoesNotContain<T>(IEnumerable<T> collection, T item, IEqualityComparer<T> comparer, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        if (collection.Contains(item, comparer))
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "DoesNotContain",
                $"Expected not to contain: {AssertionMessageFormatter.FormatValue(item)}"));
        }
    }

    /// <summary>
    /// Verifies that two collections are equal by comparing their elements <b>in order</b>, using the
    /// supplied comparer to determine element equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="expected">The expected collection.</param>
    /// <param name="actual">The actual collection.</param>
    /// <param name="comparer">The comparer used to determine whether corresponding elements are equal.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collections are not equal.</exception>
    public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string message)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

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
            if (!comparer.Equals(expectedList[i], actualList[i]))
            {
                throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEqual"));
            }
        }
    }

    /// <summary>
    /// Verifies that two collections are equal by comparing their elements <b>in order</b>, running a
    /// custom assertion against each corresponding pair of elements.
    /// </summary>
    /// <remarks>
    /// Use this when equality cannot be expressed as a simple <see cref="IEqualityComparer{T}"/> and
    /// you want to assert on individual members of each element (for example comparing two DTOs
    /// property-by-property). The <paramref name="assertElement"/> action receives
    /// <c>(expectedElement, actualElement)</c> and should throw when they do not match — typically by
    /// calling other <c>AssertThat</c> members or fluent <c>Should*</c> assertions. Any exception it
    /// throws is wrapped in an <see cref="AssertionException"/> that identifies the failing index.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="expected">The expected collection.</param>
    /// <param name="actual">The actual collection.</param>
    /// <param name="assertElement">The assertion to run against each <c>(expected, actual)</c> element pair.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collections differ in length or any element pair fails the assertion.</exception>
    public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, Action<T, T> assertElement, string message)
    {
        if (assertElement == null)
        {
            throw new ArgumentNullException(nameof(assertElement));
        }

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
            try
            {
                assertElement(expectedList[i], actualList[i]);
            }
            catch (Exception ex)
            {
                throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(
                    expectedList[i], actualList[i],
                    $"{message} -- custom comparison failed for element at index {i}: {ex.Message}",
                    "AreEqual"));
            }
        }
    }

    /// <summary>
    /// Verifies that two collections contain the same elements <b>regardless of order</b> (multiset /
    /// bag equality). Duplicate values must appear the same number of times in both collections.
    /// </summary>
    /// <remarks>
    /// Elements are matched by value using <see cref="EqualityComparer{T}.Default"/> (see the
    /// class-level remarks). Use
    /// <see cref="AreEquivalent{T}(IEnumerable{T}, IEnumerable{T}, IEqualityComparer{T}, string)"/> to
    /// supply your own equality. To require matching order as well, use
    /// <see cref="AreEqual{T}(IEnumerable{T}, IEnumerable{T}, string)"/>.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="expected">The expected collection.</param>
    /// <param name="actual">The actual collection.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collections are not equivalent.</exception>
    public static void AreEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message)
    {
        AreEquivalent(expected, actual, EqualityComparer<T>.Default, message);
    }

    /// <summary>
    /// Verifies that two collections contain the same elements <b>regardless of order</b> (multiset /
    /// bag equality), using the supplied comparer to determine element equality. Duplicate values
    /// must appear the same number of times in both collections.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="expected">The expected collection.</param>
    /// <param name="actual">The actual collection.</param>
    /// <param name="comparer">The comparer used to match elements between the collections.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collections are not equivalent.</exception>
    public static void AreEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string message)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        if (expected == null && actual == null)
        {
            return;
        }

        if (expected == null || actual == null)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEquivalent"));
        }

        var expectedList = expected.ToList();
        var remaining = actual.ToList();

        if (expectedList.Count != remaining.Count)
        {
            throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual, message, "AreEquivalent"));
        }

        foreach (var item in expectedList)
        {
            var index = remaining.FindIndex(candidate => comparer.Equals(item, candidate));

            if (index < 0)
            {
                throw new AssertionException(AssertionMessageFormatter.FormatComparisonMessage(expected, actual,
                    $"{message} -- missing expected element (or wrong number of occurrences): {AssertionMessageFormatter.FormatValue(item)}",
                    "AreEquivalent"));
            }

            remaining.RemoveAt(index);
        }
    }

    /// <summary>
    /// Verifies that the collection is a subset of the superset collection, using the supplied
    /// comparer to determine membership.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="subset">The collection that should be a subset.</param>
    /// <param name="superset">The collection that should contain all elements of the subset.</param>
    /// <param name="comparer">The comparer used to match subset elements against the superset.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the subset is not contained in the superset.</exception>
    public static void IsSubsetOf<T>(IEnumerable<T> subset, IEnumerable<T> superset, IEqualityComparer<T> comparer, string message)
    {
        if (subset == null)
        {
            throw new ArgumentNullException(nameof(subset));
        }

        if (superset == null)
        {
            throw new ArgumentNullException(nameof(superset));
        }

        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        var supersetList = superset.ToList();
        var missingItems = subset.Where(item => !supersetList.Contains(item, comparer)).ToList();

        if (missingItems.Any())
        {
            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(subset, message, "IsSubsetOf",
                $"Missing items in superset: {string.Join(", ", missingItems.Select(item => AssertionMessageFormatter.FormatValue(item)))}"));
        }
    }

    /// <summary>
    /// Verifies that the collection is a superset of the subset collection, using the supplied
    /// comparer to determine membership.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="superset">The collection that should contain all elements of the subset.</param>
    /// <param name="subset">The collection that should be contained in the superset.</param>
    /// <param name="comparer">The comparer used to match subset elements against the superset.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the superset does not contain all elements of the subset.</exception>
    public static void IsSupersetOf<T>(IEnumerable<T> superset, IEnumerable<T> subset, IEqualityComparer<T> comparer, string message)
    {
        IsSubsetOf(subset, superset, comparer, message);
    }

    /// <summary>
    /// Verifies that the collection contains only unique elements, using the supplied comparer to
    /// determine equality.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="comparer">The comparer used to determine whether two elements are duplicates.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when the collection contains duplicate elements.</exception>
    public static void HasUniqueElements<T>(IEnumerable<T> collection, IEqualityComparer<T> comparer, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        var collectionList = collection.ToList();
        var uniqueElements = collectionList.Distinct(comparer).ToList();

        if (collectionList.Count != uniqueElements.Count)
        {
            var duplicates = collectionList.GroupBy(x => x, comparer)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "HasUniqueElements",
                $"Found duplicate elements: {string.Join(", ", duplicates.Select(item => AssertionMessageFormatter.FormatValue(item)))}"));
        }
    }

    /// <summary>
    /// Verifies that every element in the collection satisfies the supplied assertion.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="AllMatch{T}(IEnumerable{T}, Func{T, bool}, string)"/>, which takes a boolean
    /// predicate, <paramref name="assertion"/> is an <see cref="Action{T}"/> that performs one or more
    /// rich assertions against each element (for example fluent <c>Should*</c> calls). If the action
    /// throws for any element, the failure is wrapped in an <see cref="AssertionException"/> that
    /// identifies the failing index and element, preserving the inner assertion's message.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <param name="assertion">The assertion to run against each element.</param>
    /// <param name="message">The message to display if the assertion fails.</param>
    /// <exception cref="AssertionException">Thrown when any element fails the assertion.</exception>
    public static void AllSatisfy<T>(IEnumerable<T> collection, Action<T> assertion, string message)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (assertion == null)
        {
            throw new ArgumentNullException(nameof(assertion));
        }

        var list = collection.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                assertion(list[i]);
            }
            catch (Exception ex)
            {
                throw new AssertionException(AssertionMessageFormatter.FormatCollectionMessage(collection, message, "AllSatisfy",
                    $"Element at index {i} ({AssertionMessageFormatter.FormatValue(list[i])}) did not satisfy the assertion: {ex.Message}"));
            }
        }
    }
}