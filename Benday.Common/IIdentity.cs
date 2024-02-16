namespace Benday.Common;

public interface IIdentity<T>
{
    T Id { get; set; }
}