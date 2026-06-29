using System;
using System.Text;

namespace Benday.Common.Testing;

/// <summary>
/// Generates fake values for populating test data. Each value is derived from a
/// supplied field name so that the generated data is distinct per field and easy
/// to trace back to a specific property when an assertion fails.
/// </summary>
public static class FakeValueGenerator
{
    /// <summary>
    /// Generates a fake <see cref="int"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake integer value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static int GetFakeValueForInt(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return Random.Shared.Next() + forFieldName.Length;
    }

    /// <summary>
    /// Generates a fake <see cref="double"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake double value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static double GetFakeValueForDouble(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return Random.Shared.NextDouble() + forFieldName.Length;
    }

    /// <summary>
    /// Generates a fake <see cref="float"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake float value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static float GetFakeValueForFloat(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return Convert.ToSingle(Random.Shared.NextDouble()) + forFieldName.Length;
    }

    /// <summary>
    /// Generates a fake <see cref="byte"/> array for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake byte array derived from the UTF-8 encoding of the field name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static byte[] GetFakeValueForByteArray(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return Encoding.UTF8.GetBytes(forFieldName);
    }

    /// <summary>
    /// Generates a fake <see cref="string"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake string value that incorporates the field name and a tick count for uniqueness.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static string GetFakeValueForString(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return string.Format("{0}_{1}", forFieldName, DateTime.Now.Ticks);
    }

    /// <summary>
    /// Generates a fake URL value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake URL string that incorporates the field name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static string GetFakeValueForUrl(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return string.Format(
            "http://www.fakestuff.com/images/{0}.jpg",
            forFieldName);
    }

    /// <summary>
    /// Generates a fake <see cref="DateTime"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <returns>A fake date that is offset from the current time by the length of the field name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static DateTime GetFakeValueForDateTime(string forFieldName)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return DateTime.Now.AddMinutes(forFieldName.Length);
    }
}