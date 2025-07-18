namespace Benday.Common.Testing;

public static class CollectionAssertionExtensions
{
    public static ICheckCollectionAssertion<ICollection<T>> IsEqualTo<T>(
        this ICheckCollectionAssertion<ICollection<T>> check, ICollection<T> expected)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        if (!check.Input.SequenceEqual(expected))
        {
            check.FailWithOptionalMessage($"Expected collection to equal: [{string.Join(", ", expected)}]");
        }
        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> IsNotEqualTo<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, IEnumerable<T> notExpected)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        if (check.Input.SequenceEqual(notExpected))
        {
            check.FailWithOptionalMessage($"Did not expect collection to equal: [{string.Join(", ", notExpected)}]");
        }
        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> IsEquivalentTo<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, IEnumerable<T> expected)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var expectedSet = new HashSet<T>(expected);

        if (!actualSet.SetEquals(expectedSet))
        {
            check.FailWithOptionalMessage($"Expected collection to be equivalent to: [{string.Join(", ", expected)}]");
        }

        return check;
    }

    public static ICheckCollectionAssertion<IEnumerable<T>> IsNotEquivalentTo<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, IEnumerable<T> notExpected)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var expectedSet = new HashSet<T>(notExpected);

        if (actualSet.SetEquals(expectedSet))
        {
            check.FailWithOptionalMessage($"Did not expect collection to be equivalent to: [{string.Join(", ", notExpected)}]");
        }

        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> Contains<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, T expected)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        if (!check.Input.Contains(expected))
        {
            check.FailWithOptionalMessage($"Expected collection to contain: {expected}");
        }
        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> DoesNotContain<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, T unexpected)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        if (check.Input.Contains(unexpected))
        {
            check.FailWithOptionalMessage($"Did not expect collection to contain: {unexpected}");
        }
        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T?>> AllItemsAreNotNull<T>(this ICheckCollectionAssertion<IEnumerable<T?>> check) where T : class
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        foreach (var item in check.Input)
        {
            if (item == null)
            {
                check.FailWithOptionalMessage("Expected all items to be non-null.");
            }
        }

        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> AllItemsAreUnique<T>(this ICheckCollectionAssertion<IEnumerable<T>> check)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        var set = new HashSet<T>();
        foreach (var item in check.Input)
        {
            if (!set.Add(item))
            {
                check.FailWithOptionalMessage($"Expected all items to be unique, but found duplicate: {item}");
            }
        }

        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> IsSubsetOf<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, IEnumerable<T> superset)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var supersetSet = new HashSet<T>(superset);

        if (!actualSet.IsSubsetOf(supersetSet))
        {
            check.FailWithOptionalMessage("Expected collection to be a subset of the specified superset.");
        }

        return check;
    }


    public static ICheckCollectionAssertion<IEnumerable<T>> IsSupersetOf<T>(this ICheckCollectionAssertion<IEnumerable<T>> check, IEnumerable<T> subset)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var subsetSet = new HashSet<T>(subset);

        if (!actualSet.IsSupersetOf(subsetSet))
        {
            check.FailWithOptionalMessage("Expected collection to be a superset of the specified subset.");
        }

        return check;
    }

}
