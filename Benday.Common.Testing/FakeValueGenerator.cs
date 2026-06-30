using System;
using System.Text;

namespace Benday.Common.Testing;

/// <summary>
/// Generates fake values for populating test data. Each value is derived from a
/// supplied field name so that the generated data is distinct per field and easy
/// to trace back to a specific property when an assertion fails.
/// <para>
/// Every generator supports two modes. In <em>predictable</em> mode (the default)
/// the returned value is a deterministic function of the field name and
/// <c>seedValue</c>, so the same inputs always produce the same output. In
/// <em>randomize</em> mode the value is non-deterministic. When building a
/// collection of objects, pass the item's index as <c>seedValue</c> to keep the
/// predictable values unique across the collection.
/// </para>
/// </summary>
public static partial class FakeValueGenerator
{
    /// <summary>
    /// The fixed base date used to generate predictable <see cref="DateTime"/> values.
    /// </summary>
    private static readonly DateTime PredictableBaseDate = new(2000, 1, 1);

    /// <summary>
    /// Generates a fake <see cref="int"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random value; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake integer value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static int GetFakeValueForInt(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return Random.Shared.Next() + forFieldName.Length;
        }
        else
        {
            return forFieldName.Length + seedValue;
        }
    }

    /// <summary>
    /// Generates a fake <see cref="double"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random value; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake double value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static double GetFakeValueForDouble(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return Random.Shared.NextDouble() + forFieldName.Length;
        }
        else
        {
            return forFieldName.Length + seedValue;
        }
    }

    /// <summary>
    /// Generates a fake <see cref="float"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random value; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake float value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static float GetFakeValueForFloat(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return Convert.ToSingle(Random.Shared.NextDouble()) + forFieldName.Length;
        }
        else
        {
            return forFieldName.Length + seedValue;
        }
    }

    /// <summary>
    /// Generates a fake <see cref="byte"/> array for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns random bytes; otherwise returns a predictable byte array derived from the field name and seed.</param>
    /// <param name="seedValue">A value that makes predictable byte arrays unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static byte[] GetFakeValueForByteArray(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            var bytes = new byte[16];
            Random.Shared.NextBytes(bytes);
            return bytes;
        }
        else
        {
            return Encoding.UTF8.GetBytes($"{forFieldName}_{seedValue}");
        }
    }

    /// <summary>
    /// Generates a fake <see cref="string"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a value that incorporates a tick count; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">A value that makes predictable strings unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake string value that incorporates the field name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static string GetFakeValueForString(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return $"{forFieldName}_{DateTime.Now.Ticks}";
        }
        else
        {
            return $"{forFieldName}_{seedValue}";
        }
    }

    /// <summary>
    /// Generates a fake URL value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a value that incorporates a tick count; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">A value that makes predictable URLs unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake URL string that incorporates the field name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static string GetFakeValueForUrl(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        var uniquePart = randomize ? DateTime.Now.Ticks.ToString() : seedValue.ToString();

        return $"http://www.fakestuff.com/images/{forFieldName}_{uniquePart}.jpg";
    }

    /// <summary>
    /// Generates a fake <see cref="DateTime"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a value offset from the current time; otherwise returns a predictable value offset from a fixed base date.</param>
    /// <param name="seedValue">An offset (in minutes) that makes predictable dates unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake date.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static DateTime GetFakeValueForDateTime(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return DateTime.Now.AddMinutes(forFieldName.Length);
        }
        else
        {
            return PredictableBaseDate.AddMinutes(forFieldName.Length + seedValue);
        }
    }

    /// <summary>
    /// Generates a fake <see cref="long"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random value; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake long value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static long GetFakeValueForLong(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return Random.Shared.NextInt64() + forFieldName.Length;
        }
        else
        {
            return forFieldName.Length + (long)seedValue;
        }
    }

    /// <summary>
    /// Generates a fake <see cref="bool"/> value for the supplied field name. This always returns
    /// <c>true</c>, regardless of <paramref name="randomize"/> or <paramref name="seedValue"/>.
    /// </summary>
    /// <remarks>
    /// <c>false</c> is <c>default(bool)</c>, so it is the only boolean value that is indistinguishable
    /// from an unset property. Returning it would defeat the purpose of generating a fake value: callers
    /// pair this with <see cref="AssertThat.AllPropertiesAreNonNullAndNonDefaultValue{T}"/> to confirm a
    /// property was populated, and a fake <c>false</c> would be reported as a default. A <c>bool</c>
    /// also cannot be made unique across a collection (it only has two values), so the seed offers no
    /// benefit here. Returning <c>true</c> keeps fake booleans reliably non-default.
    /// </remarks>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">Ignored; retained for signature consistency with the other generators.</param>
    /// <param name="seedValue">Ignored; retained for signature consistency with the other generators.</param>
    /// <returns>Always <c>true</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static bool GetFakeValueForBool(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        return true;
    }

    /// <summary>
    /// Generates a fake <see cref="decimal"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random value; otherwise returns a predictable value derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake decimal value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static decimal GetFakeValueForDecimal(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return (decimal)Random.Shared.NextDouble() + forFieldName.Length;
        }
        else
        {
            return forFieldName.Length + seedValue;
        }
    }

    /// <summary>
    /// Generates a fake <see cref="Guid"/> value for the supplied field name.
    /// </summary>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a new random Guid; otherwise returns a predictable Guid derived from the field name and seed.</param>
    /// <param name="seedValue">A value that makes predictable Guids unique across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake Guid value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static Guid GetFakeValueForGuid(string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (randomize)
        {
            return Guid.NewGuid();
        }
        else
        {
            var source = Encoding.UTF8.GetBytes($"{forFieldName}_{seedValue}");
            var buffer = new byte[16];

            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = source[i % source.Length];
            }

            return new Guid(buffer);
        }
    }

    /// <summary>
    /// Generates a fake enum value of type <typeparamref name="TEnum"/> for the supplied field name.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to generate a value for.</typeparam>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random member; otherwise returns a predictable member derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values cycle across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake enum value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="forFieldName"/> is null.</exception>
    public static TEnum GetFakeValueForEnum<TEnum>(string forFieldName, bool randomize = false, int seedValue = 0)
        where TEnum : struct, Enum
    {
        return (TEnum)GetFakeValueForEnum(typeof(TEnum), forFieldName, randomize, seedValue);
    }

    /// <summary>
    /// Generates a fake enum value of the supplied enum type for the supplied field name. This
    /// overload is intended for reflection-based scenarios where the enum type is not known at compile time.
    /// </summary>
    /// <param name="enumType">The enum type to generate a value for.</param>
    /// <param name="forFieldName">The name of the field the value is being generated for.</param>
    /// <param name="randomize">When true, returns a random member; otherwise returns a predictable member derived from the field name and seed.</param>
    /// <param name="seedValue">An offset that makes predictable values cycle across a collection (e.g. the item index). Ignored when <paramref name="randomize"/> is true.</param>
    /// <returns>A fake enum value boxed as an <see cref="object"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumType"/> or <paramref name="forFieldName"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="enumType"/> is not an enum type or has no members.</exception>
    public static object GetFakeValueForEnum(Type enumType, string forFieldName, bool randomize = false, int seedValue = 0)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(forFieldName);

        if (enumType.IsEnum == false)
        {
            throw new ArgumentException($"Type '{enumType.FullName}' is not an enum type.", nameof(enumType));
        }

        var values = Enum.GetValues(enumType);

        if (values.Length == 0)
        {
            throw new ArgumentException($"Enum type '{enumType.FullName}' has no members.", nameof(enumType));
        }

        var index = randomize
            ? Random.Shared.Next(values.Length)
            : (forFieldName.Length + seedValue) % values.Length;

        return values.GetValue(index)!;
    }
}