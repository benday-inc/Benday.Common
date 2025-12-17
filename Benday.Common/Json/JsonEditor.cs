using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Benday.Common.Json;

/// <summary>
/// A utility class for reading and editing JSON documents using path-based navigation.
/// </summary>
public class JsonEditor
{
    private readonly JsonNode _rootNode;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonEditor"/> class by loading JSON from a file.
    /// </summary>
    /// <param name="filePath">The path to the JSON file to load.</param>
    public JsonEditor(string filePath) : this(File.ReadAllText(filePath), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonEditor"/> class from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <param name="loadFromString">Must be true. This parameter exists to differentiate from the file path constructor.</param>
    /// <exception cref="InvalidOperationException">Thrown when loadFromString is false or JSON cannot be parsed.</exception>
    /// <exception cref="ArgumentException">Thrown when json is null or empty.</exception>
    public JsonEditor(string json, bool loadFromString)
    {
        if (loadFromString == false)
        {
            throw new InvalidOperationException("Argument not valid on this constructor.");
        }

        if (string.IsNullOrEmpty(json))
            throw new ArgumentException($"{nameof(json)} is null or empty.", nameof(json));

        var temp = JsonObject.Parse(json);

        if (temp != null)
        {
            _rootNode = temp;
        }
        else
        {
            throw new InvalidOperationException($"Could not parse JsonNode from json.");
        }
    }

    /// <summary>
    /// Gets the string value at the specified JSON path.
    /// </summary>
    /// <param name="nodes">The path to the value as an array of node names.</param>
    /// <returns>The value as a string, or null if not found.</returns>
    /// <exception cref="ArgumentException">Thrown when nodes is null or empty.</exception>
    public string? GetValue(params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var node = GetNode(nodes);

        if (node == null)
        {
            return null;
        }
        else
        {
            return node.ToString();
        }
    }

    /// <summary>
    /// Gets the boolean value at the specified JSON path.
    /// </summary>
    /// <param name="nodes">The path to the value as an array of node names.</param>
    /// <returns>The value as a boolean, or null if not found or cannot be parsed.</returns>
    /// <exception cref="ArgumentException">Thrown when nodes is null or empty.</exception>
    public bool? GetValueAsBoolean(params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var node = GetNode(nodes);

        if (node == null)
        {
            return null;
        }
        else
        {
            if (bool.TryParse(node.ToString(), out bool result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Gets the integer value at the specified JSON path.
    /// </summary>
    /// <param name="nodes">The path to the value as an array of node names.</param>
    /// <returns>The value as an integer, or null if not found or cannot be parsed.</returns>
    /// <exception cref="ArgumentException">Thrown when nodes is null or empty.</exception>
    public int? GetValueAsInt32(params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var node = GetNode(nodes);

        if (node == null)
        {
            return null;
        }
        else
        {
            if (int.TryParse(node.ToString(), out int result) == false)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
    }

    private JsonNode? GetNode(params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var parent = _rootNode;

        if (parent == null)
        {
            throw new InvalidOperationException($"Root node was null.");
        }

        JsonNode? node;
        bool success = false;

        for (int index = 0; index < nodes.Length; index++)
        {
            node = parent![nodes[index]];

            if (node == null)
            {
                success = false;
            }
            else
            {
                success = true;
            }

            if (success == false)
            {
                return null;
            }
            else if (index == nodes.Length - 1)
            {
                // found what we want
                return node;
            }
            else
            {
                // keep searching
                parent = node;
            }
        }

        return null;
    }

    /// <summary>
    /// Sets a string value at the specified JSON path, creating intermediate nodes if necessary.
    /// </summary>
    /// <param name="nodeValue">The string value to set.</param>
    /// <param name="nodes">The path to the value as an array of node names.</param>
    /// <exception cref="ArgumentException">Thrown when nodeValue or nodes is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the parent node is null.</exception>
    public void SetValue(string nodeValue, params string[] nodes)
    {
        if (string.IsNullOrEmpty(nodeValue))
            throw new ArgumentException($"{nameof(nodeValue)} is null or empty.", nameof(nodeValue));
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var match = GetNode(nodes);

        if (match == null)
        {
            CreateNodeStructureAndSetValue(nodeValue, nodes);
        }
        else
        {
            var propertyName = nodes.Last();

            var parent = match.Parent;

            if (parent == null)
            {
                throw new InvalidOperationException($"Parent is null");
            }

            parent[propertyName] = nodeValue;
        }
    }

    /// <summary>
    /// Sets a boolean value at the specified JSON path, creating intermediate nodes if necessary.
    /// </summary>
    /// <param name="nodeValue">The boolean value to set.</param>
    /// <param name="nodes">The path to the value as an array of node names.</param>
    /// <exception cref="ArgumentException">Thrown when nodes is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the parent node is null.</exception>
    public void SetValue(bool nodeValue, params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var match = GetNode(nodes);

        if (match == null)
        {
            CreateNodeStructureAndSetValue(nodeValue, nodes);
        }
        else
        {
            var propertyName = nodes.Last();

            var parent = match.Parent;

            if (parent == null)
            {
                throw new InvalidOperationException($"Parent is null");
            }

            parent[propertyName] = nodeValue;
        }
    }

    /// <summary>
    /// Sets an integer value at the specified JSON path, creating intermediate nodes if necessary.
    /// </summary>
    /// <param name="nodeValue">The integer value to set.</param>
    /// <param name="nodes">The path to the value as an array of node names.</param>
    /// <exception cref="ArgumentException">Thrown when nodes is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the parent node is null.</exception>
    public void SetValue(int nodeValue, params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var match = GetNode(nodes);

        if (match == null)
        {
            CreateNodeStructureAndSetValue(nodeValue, nodes);
        }
        else
        {
            var propertyName = nodes.Last();

            var parent = match.Parent;

            if (parent == null)
            {
                throw new InvalidOperationException($"Parent is null");
            }

            parent[propertyName] = nodeValue;
        }
    }

    private void CreateNodeStructureAndSetValue(string nodeValue, params string[] nodes)
    {
        if (string.IsNullOrEmpty(nodeValue))
            throw new ArgumentException($"{nameof(nodeValue)} is null or empty.", nameof(nodeValue));
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var parent = _rootNode;

        JsonNode? node;

        for (int index = 0; index < nodes.Length; index++)
        {
            node = parent![nodes[index]];

            if (node != null)
            {
                parent = node;
            }
            else
            {
                if (index == nodes.Length - 1)
                {
                    // set the value
                    parent[nodes[index]] = nodeValue;
                }
                else
                {
                    node = new JsonObject();

                    parent[nodes[index]] = node;

                    parent = node;
                }
            }
        }
    }

    private void CreateNodeStructureAndSetValue(bool nodeValue, params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var parent = _rootNode;

        JsonNode? node;

        for (int index = 0; index < nodes.Length; index++)
        {
            node = parent![nodes[index]];

            if (node != null)
            {
                parent = node;
            }
            else
            {
                if (index == nodes.Length - 1)
                {
                    // set the value
                    parent[nodes[index]] = nodeValue;
                }
                else
                {
                    node = new JsonObject();

                    parent[nodes[index]] = node;

                    parent = node;
                }
            }
        }
    }

    private void CreateNodeStructureAndSetValue(int nodeValue, params string[] nodes)
    {
        if (nodes == null || nodes.Length == 0)
            throw new ArgumentException(
            $"{nameof(nodes)} is null or empty.", nameof(nodes));

        var parent = _rootNode;

        JsonNode? node;

        for (int index = 0; index < nodes.Length; index++)
        {
            node = parent![nodes[index]];

            if (node != null)
            {
                parent = node;
            }
            else
            {
                if (index == nodes.Length - 1)
                {
                    // set the value
                    parent[nodes[index]] = nodeValue;
                }
                else
                {
                    node = new JsonObject();

                    parent[nodes[index]] = node;

                    parent = node;
                }
            }
        }
    }

    /// <summary>
    /// Gets a sibling value from a JSON array based on a search key/value pair.
    /// </summary>
    /// <param name="args">The arguments specifying the search criteria and desired value.</param>
    /// <returns>The sibling value as a string, or null if not found.</returns>
    public string? GetSiblingValue(SiblingValueArguments args)
    {
        var parentNode = FindParentNodeBySiblingValue(args);

        if (parentNode == null)
        {
            return null;
        }
        else
        {
            var match = parentNode[args.DesiredNodeKey];

            if (match == null)
            {
                return null;
            }
            else
            {
                return match.ToString();
            }
        }
    }

    /// <summary>
    /// Sets a sibling value in a JSON array based on a search key/value pair.
    /// </summary>
    /// <param name="args">The arguments specifying the search criteria and value to set.</param>
    public void SetSiblingValue(SiblingValueArguments args)
    {
        var parentNode = FindParentNodeBySiblingValue(args);

        if (parentNode == null)
        {
            return;
        }
        else
        {
            parentNode[args.DesiredNodeKey] = args.DesiredNodeValue;
        }
    }

    private JsonNode? FindParentNodeBySiblingValue(SiblingValueArguments args)
    {
        var collectionMatch = GetNode(args.PathArguments);

        if (collectionMatch == null)
        {
            return null;
        }
        else if (collectionMatch is JsonArray)
        {
            var matches = (JsonArray)collectionMatch;

            foreach (var item in matches)
            {
                if (item == null)
                {
                    continue;
                }
                else if (item[args.SiblingSearchKey] != null &&
                    item[args.SiblingSearchKey]!.ToString() == args.SiblingSearchValue)
                {
                    return item;
                }
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Converts the JSON document to a string.
    /// </summary>
    /// <param name="indented">If true, the output will be formatted with indentation.</param>
    /// <returns>The JSON document as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the root node is null.</exception>
    public string ToJson(bool indented = false)
    {
        if (_rootNode == null)
        {
            throw new InvalidOperationException($"Root node is null");
        }
        else
        {
            if (indented == true)
            {
                return _rootNode.ToJsonString(
                    new JsonSerializerOptions() { WriteIndented = true });
            }
            else
            {
                return _rootNode.ToJsonString();
            }
        }
    }
}
