using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Common.Testing;
public static class NullableValueTypeAssertionExtensions
{
    public static ICheckAssertionForNullableType<int?> IsNotZero(
        this ICheckAssertionForNullableType<int?> check,
        string? userFailureMessage = null)
    {
        if (check.Input is null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (check.Input == 0)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input should not be zero.");
        }

        return check;
    }

    public static ICheckAssertionForNullableType<int?> IsZero(
        this ICheckAssertionForNullableType<int?> check,
        string? userFailureMessage = null)
    {
        if (check.Input is null)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input is null.");
        }

        if (check.Input != 0)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input should be zero.");
        }

        return check;
    }


    public static ICheckAssertion<int> IsNotZero(
        this ICheckAssertion<int> check,
        string? userFailureMessage = null)
    {        
        if (check.Input == 0)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input should not be zero.");
        }

        return check;
    }

    public static ICheckAssertion<int> IsZero(
        this ICheckAssertion<int> check,
        string? userFailureMessage = null)
    {
        if (check.Input != 0)
        {
            check.FailWithOptionalMessage(userFailureMessage, "Input should be zero.");
        }

        return check;
    }
}
