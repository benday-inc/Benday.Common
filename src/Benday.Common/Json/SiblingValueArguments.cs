using System;

namespace Benday.Common.Json;

/// <summary>
/// Arguments for finding and setting sibling values in a JSON array.
/// Used with <see cref="JsonEditor.GetSiblingValue"/> and <see cref="JsonEditor.SetSiblingValue"/>.
/// </summary>
public class SiblingValueArguments
{
    /// <summary>
    /// Gets or sets the value to search for in the sibling node.
    /// </summary>
    public string SiblingSearchValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the key name of the sibling node to search.
    /// </summary>
    public string SiblingSearchKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path to the JSON array containing the items to search.
    /// </summary>
    public string[] PathArguments { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Gets or sets the key name of the desired node to get or set.
    /// </summary>
    public string DesiredNodeKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value to set on the desired node (used with SetSiblingValue).
    /// </summary>
    public string DesiredNodeValue { get; set; } = string.Empty;
}
