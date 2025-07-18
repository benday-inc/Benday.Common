namespace Benday.Common.Testing;

public static class ArrayAssertionExtensions
{
    public static ICheckAssertion<T[]> IsEqualTo<T>(this ICheckAssertion<T[]> check, IEnumerable<T> expected)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).IsEqualTo(expected);
        return check;
    }

    public static ICheckAssertion<T[]> IsNotEqualTo<T>(this ICheckAssertion<T[]> check, IEnumerable<T> notExpected)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).IsNotEqualTo(notExpected);
        return check;
    }

    public static ICheckAssertion<T[]> IsEquivalentTo<T>(this ICheckAssertion<T[]> check, IEnumerable<T> expected)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).IsEquivalentTo(expected);
        return check;
    }

    public static ICheckAssertion<T[]> IsNotEquivalentTo<T>(this ICheckAssertion<T[]> check, IEnumerable<T> notExpected)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).IsNotEquivalentTo(notExpected);
        return check;
    }

    public static ICheckAssertion<T[]> Contains<T>(this ICheckAssertion<T[]> check, T expected)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).Contains(expected);
        return check;
    }

    public static ICheckAssertion<T[]> DoesNotContain<T>(this ICheckAssertion<T[]> check, T unexpected)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).DoesNotContain(unexpected);
        return check;
    }

    public static ICheckAssertion<T[]> AllItemsAreUnique<T>(this ICheckAssertion<T[]> check)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).AllItemsAreUnique();
        return check;
    }

    public static ICheckAssertion<T[]> IsSubsetOf<T>(this ICheckAssertion<T[]> check, IEnumerable<T> superset)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).IsSubsetOf(superset);
        return check;
    }

    public static ICheckAssertion<T[]> IsSupersetOf<T>(this ICheckAssertion<T[]> check, IEnumerable<T> subset)
    {
        ((ICheckAssertion<IEnumerable<T>>)check.AsEnumerableWrapper()).IsSupersetOf(subset);
        return check;
    }

    private static ICheckAssertion<IEnumerable<T>> AsEnumerableWrapper<T>(this ICheckAssertion<T[]> check)
    {
        if (check.Input == null)
        {
            check.FailWithOptionalMessage("Actual array is null.");
        }

        return new CheckAssertion<IEnumerable<T>>(check.Input);
    }
    
    public static ICheckAssertion<T[]> AllItemsAreNotNull<T>(this ICheckAssertion<T[]> check)
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
