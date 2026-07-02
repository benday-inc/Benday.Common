using System.Text.Json;

namespace Benday.Common.Json;

/// <summary>
/// Represents the result of an attempt to locate a <see cref="JsonElement"/> by navigating
/// a set of property names.
/// </summary>
public class ElementResult
{
    /// <summary>
    /// Indicates whether the requested element was found.
    /// </summary>
    public bool Found { get; set; }

    /// <summary>
    /// The located <see cref="JsonElement"/>. This value is only meaningful when
    /// <see cref="Found"/> is true.
    /// </summary>
    public JsonElement Element { get; set; }
}