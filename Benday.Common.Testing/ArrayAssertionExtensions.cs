namespace Benday.Common.Testing;

public static class ArrayAssertionExtensions
{
    public static ICheckArrayAssertion<T[]> IsEqualTo<T>(
        this ICheckArrayAssertion<T[]> check, IEnumerable<T> expected,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        if (!check.Input.SequenceEqual(expected))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected collection to equal: [{string.Join(", ", expected)}]");
        }
        return check;
    }


    public static ICheckArrayAssertion<T[]> IsNotEqualTo<T>(
        this ICheckArrayAssertion<T[]> check, IEnumerable<T> notExpected,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        if (check.Input.SequenceEqual(notExpected))
        {
            check.FailWithOptionalMessage(userFailureMessage,
            $"Did not expect collection to equal: [{string.Join(", ", notExpected)}]");
        }
        return check;
    }


    public static ICheckArrayAssertion<T[]> IsEquivalentTo<T>(
        this ICheckArrayAssertion<T[]> check,
        IEnumerable<T> expected,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var expectedSet = new HashSet<T>(expected);

        if (!actualSet.SetEquals(expectedSet))
        {
            check.FailWithOptionalMessage(userFailureMessage,
                $"Expected collection to be equivalent to: [{string.Join(", ", expected)}]");
        }

        return check;
    }

    public static ICheckArrayAssertion<T[]> IsNotEquivalentTo<T>(
        this ICheckArrayAssertion<T[]> check, IEnumerable<T> notExpected,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var expectedSet = new HashSet<T>(notExpected);

        if (actualSet.SetEquals(expectedSet))
        {
            check.FailWithOptionalMessage(
                userFailureMessage,
                $"Did not expect collection to be equivalent to: [{string.Join(", ", notExpected)}]");
        }

        return check;
    }

    public static ICheckArrayAssertion<T[]> Contains<T>(
        this ICheckArrayAssertion<T[]> check, T expected,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        if (!check.Input.Contains(expected))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Expected collection to contain: {expected}");
        }
        return check;
    }


    public static ICheckArrayAssertion<T[]> DoesNotContain<T>(
        this ICheckArrayAssertion<T[]> check, T unexpected,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        if (check.Input.Contains(unexpected))
        {
            check.FailWithOptionalMessage(userFailureMessage, $"Did not expect collection to contain: {unexpected}");
        }
        return check;
    }


    public static ICheckArrayAssertion<T[]> AllItemsAreNotNull<T>(
        this ICheckArrayAssertion<T[]> check,
            string? userFailureMessage = null) where T : class
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        foreach (var item in check.Input)
        {
            if (item == null)
            {
                check.FailWithOptionalMessage(userFailureMessage, "Expected all items to be non-null.");
            }
        }

        return check;
    }


    public static ICheckArrayAssertion<T[]> AllItemsAreUnique<T>(
        this ICheckArrayAssertion<T[]> check,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        var set = new HashSet<T>();
        foreach (var item in check.Input)
        {
            if (!set.Add(item))
            {
                check.FailWithOptionalMessage(userFailureMessage, $"Expected all items to be unique, but found duplicate: {item}");
            }
        }

        return check;
    }


    public static ICheckArrayAssertion<T[]> IsSubsetOf<T>(
        this ICheckArrayAssertion<T[]> check, IEnumerable<T> superset,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var supersetSet = new HashSet<T>(superset);

        if (!actualSet.IsSubsetOf(supersetSet))
        {
            check.FailWithOptionalMessage(userFailureMessage, "Expected collection to be a subset of the specified superset.");
        }

        return check;
    }


    public static ICheckArrayAssertion<T[]> IsSupersetOf<T>(
        this ICheckArrayAssertion<T[]> check, IEnumerable<T> subset,
            string? userFailureMessage = null)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Actual collection is null.");
        }

        var actualSet = new HashSet<T>(check.Input);
        var subsetSet = new HashSet<T>(subset);

        if (!actualSet.IsSupersetOf(subsetSet))
        {
            check.FailWithOptionalMessage(userFailureMessage, "Expected collection to be a superset of the specified subset.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<T?[]> AllItemsAreNotNull<T>(
        this ICheckAssertionForNullableType<T?[]> check)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual array is null.");
        }

        foreach (var item in check.Input)
        {
            if (item is null)
            {
                check.FailWithOptionalMessage("Expected all items to be non-null.");
            }
        }

        return check;
    }

}
