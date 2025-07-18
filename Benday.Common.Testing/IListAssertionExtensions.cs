namespace Benday.Common.Testing;

public static class IListAssertionExtensions
{
    public static ICheckCollectionAssertion<IList<T>> IsEqualTo<T>(
        this ICheckCollectionAssertion<IList<T>> check, IList<T> expected)
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


    public static ICheckCollectionAssertion<IList<T>> IsNotEqualTo<T>(this ICheckCollectionAssertion<IList<T>> check, IList<T> notExpected)
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


    public static ICheckCollectionAssertion<IList<T>> IsEquivalentTo<T>(this ICheckCollectionAssertion<IList<T>> check, IList<T> expected)
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

    public static ICheckCollectionAssertion<IList<T>> IsNotEquivalentTo<T>(this ICheckCollectionAssertion<IList<T>> check, IList<T> notExpected)
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


    public static ICheckCollectionAssertion<IList<T>> Contains<T>(this ICheckCollectionAssertion<IList<T>> check, T expected)
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


    public static ICheckCollectionAssertion<IList<T>> DoesNotContain<T>(this ICheckCollectionAssertion<IList<T>> check, T unexpected)
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


    public static ICheckCollectionAssertion<IList<T?>> AllItemsAreNotNull<T>(this ICheckCollectionAssertion<IList<T?>> check) where T : class
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


    public static ICheckCollectionAssertion<IList<T>> AllItemsAreUnique<T>(this ICheckCollectionAssertion<IList<T>> check)
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


    public static ICheckCollectionAssertion<IList<T>> IsSubsetOf<T>(this ICheckCollectionAssertion<IList<T>> check, IList<T> superset)
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


    public static ICheckCollectionAssertion<IList<T>> IsSupersetOf<T>(this ICheckCollectionAssertion<IList<T>> check, IList<T> subset)
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
