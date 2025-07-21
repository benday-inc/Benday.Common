using System;
using System.Linq;

namespace Benday.Common.Testing;

public static class RefTypeAssertionExtensions
{

    //public static ICheckAssertion<T> IsNotEqualTo<T>(this ICheckAssertion<T> check,
    //    T? expected,
    //    string? userFailureMessage = null) where T : notnull
    //{
    //    if (check.Input is null && expected is null)
    //    {
    //        check.FailWithOptionalMessage(
    //            userFailureMessage,
    //            $"Values should not be equal. Expected and actual are both null.");
    //    }
    //    else if (check.Input is not null && expected is not null)
    //    {
    //        if (object.Equals(check.Input, expected) == true)
    //        {
    //            check.FailWithOptionalMessage(
    //            userFailureMessage,
    //            $"Values should not be equal. Expected '{expected}' and actual value was '{check.Input}'");
    //        }
    //    }

    //    return check;
    //}
}
