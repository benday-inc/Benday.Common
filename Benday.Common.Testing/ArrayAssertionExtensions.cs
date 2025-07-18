namespace Benday.Common.Testing;


public static class ArrayAssertionExtensions
{
    public static ICheckArrayAssertion<T[]> IsEqualTo<T>(this ICheckArrayAssertion<T[]> check, IEnumerable<T> expected)
    {
        check.IsEqualTo(expected);
        return check;
    }

    public static ICheckArrayAssertion<T[]> IsNotEqualTo<T>(this ICheckArrayAssertion<T[]> check, IEnumerable<T> notExpected)
    {
        check.IsNotEqualTo(notExpected);
        return check;
    }

    public static ICheckArrayAssertion<T[]> IsEquivalentTo<T>(this ICheckArrayAssertion<T[]> check, IEnumerable<T> expected)
    {
        check.IsEquivalentTo(expected);
        return check;
    }

    public static ICheckArrayAssertion<T[]> IsNotEquivalentTo<T>(this ICheckArrayAssertion<T[]> check, IEnumerable<T> notExpected)
    {
        check.IsNotEquivalentTo(notExpected);
        return check;
    }

    public static ICheckArrayAssertion<T[]> Contains<T>(this ICheckArrayAssertion<T[]> check, T expected)
    {
        check.Contains(expected);
        return check;
    }

    public static ICheckArrayAssertion<T[]> DoesNotContain<T>(this ICheckArrayAssertion<T[]> check, T unexpected)
    {
        check.DoesNotContain(unexpected);
        return check;
    }

    public static ICheckArrayAssertion<T[]> AllItemsAreUnique<T>(this ICheckArrayAssertion<T[]> check)
    {
        check.AllItemsAreUnique();
        return check;
    }

    public static ICheckArrayAssertion<T[]> IsSubsetOf<T>(this ICheckArrayAssertion<T[]> check, IEnumerable<T> superset)
    {
        check.IsSubsetOf(superset);
        return check;
    }

    public static ICheckArrayAssertion<T[]> IsSupersetOf<T>(this ICheckArrayAssertion<T[]> check, IEnumerable<T> subset)
    {
        check.IsSupersetOf(subset);
        return check;
    }

    public static ICheckArrayAssertion<T[]> AllItemsAreNotNull<T>(this ICheckArrayAssertion<T[]> check)
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
