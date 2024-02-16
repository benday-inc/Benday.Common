using System;

namespace Benday.Common;

public interface IIdentity<T> where T : IComparable<T>
{
    T Id { get; set; }
}