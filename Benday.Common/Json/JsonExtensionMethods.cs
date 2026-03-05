using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Benday.Common;

namespace Benday.Common.Json;

public static class JsonExtensionMethods
{
    const string DEFAULT_VALUE_STRING = "";

    /// <summary>
    /// Get a string property value from a JsonElement safely.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="propertyNames"></param>
    /// <returns></returns>
    public static string SafeGetString(
        this JsonElement input, params string[] propertyNames)
    {

        var element = input.GetElement(propertyNames);
        if (element.Found == false)
        {
            return DEFAULT_VALUE_STRING;
        }
        else
        {
            if (element.Element.ValueKind == JsonValueKind.Null)
            {
                return DEFAULT_VALUE_STRING;
            }
            else if (element.Element.ValueKind == JsonValueKind.Undefined)
            {
                return DEFAULT_VALUE_STRING;
            }
            else if (element.Element.ValueKind == JsonValueKind.String)
            {
                return element.Element.GetString().SafeToString();
            }
            else if (element.Element.ValueKind == JsonValueKind.Number)
            {
                var resultAsString = element.Element.GetRawText();      
                return resultAsString;
            }
            else if (element.Element.ValueKind == JsonValueKind.True)
            {
                return true.ToString();
            }
            else if (element.Element.ValueKind == JsonValueKind.False)
            {
                return false.ToString();
            }
            else
            {
                var resultAsString = element.Element.GetRawText();
                return resultAsString;
            }
        }
    }

    public static DateTime SafeGetDateTime(
        this JsonElement input, string propertyName,
        DateTime defaultValue = default)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            if (value.ValueKind == JsonValueKind.Null)
            {
                return defaultValue;
            }

            if (value.TryGetDateTime(out var result) == true)
            {
                return result;
            }
        }

        return defaultValue;
    }

    public static DateTime SafeGetDateTime(
        this JsonElement input, string propertyName,
        string childPropertyName,
        DateTime defaultValue = default)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            if (value.TryGetProperty(childPropertyName, out var childValue) == true)
            {
                if (childValue.ValueKind == JsonValueKind.Null)
                {
                    return defaultValue;
                }

                if (childValue.TryGetDateTime(out var result) == true)
                {
                    return result;
                }
            }
        }

        return defaultValue;
    }

    public static double SafeGetDouble(
        this JsonElement input, params string[] propertyNames
        )
    {
        var element = input.GetElement(propertyNames);
        if (element.Found == false)
        {
            return default;
        }
        else
        {

            if (element.Element.ValueKind == JsonValueKind.Null)
            {
                return default;
            }

            if (element.Element.TryGetDouble(out var result) == true)
            {
                return result;
            }
            else
            {
                return default;
            }
        }
    }
    
    public static int SafeGetInt32(
        this JsonElement input, params string[] propertyNames
        )
    {
        var element = input.GetElement(propertyNames);
        if (element.Found == false)
        {
            return default;
        }
        else
        {
            
            if (element.Element.ValueKind == JsonValueKind.Null)
            {
                return default;
            }

            if (element.Element.TryGetInt32(out var result) == true)
            {
                return result;
            }
            else
            {
                return default;
            }
        }
    }

    

    public static JsonElement GetElementOrThrow(
        this JsonElement input,
        params string[] propertyNames)
    {
        var result = GetElement(input, propertyNames);

        if (result.Found == false)
        {
            throw new InvalidOperationException(
                $"Property not found: {string.Join(".", propertyNames)}");
        }

        return result.Element;
    }

    public static bool HasProperty(
        this JsonElement input,
        string propertyName)
    {
        return input.TryGetProperty(propertyName, out var value);
    }

    /// <summary>
    /// Gets a JsonElement by navigating the specified property names.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="propertyNames"></param>
    /// <returns></returns>
    public static ElementResult GetElement(
        this JsonElement input,
        params string[] propertyNames)
    {
        var currentElement = input;
        var result = new ElementResult()
        {
            Found = false
        };

        foreach (var propertyName in propertyNames)
        {
            if (currentElement.ValueKind != JsonValueKind.Object)
            {
                result.Found = false;
                return result;
            }
            else if (currentElement.TryGetProperty(propertyName, out var childElement) == true)
            {
                currentElement = childElement;
            }
            else
            {
                result.Found = false;
                return result;
            }
        }

        result.Found = true;
        result.Element = currentElement;
        return result;
    }

    public static long SafeGetLong(
        this JsonElement input, string propertyName,
        long defaultValue = 0)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            if (value.ValueKind == JsonValueKind.Null)
            {
                return defaultValue;
            }

            if (value.TryGetInt64(out var result) == true)
            {
                return result;
            }
        }

        return defaultValue;
    }

    public static long SafeGetLong(
        this JsonElement input,
        string propertyName,
        string childPropertyName,
        long defaultValue = 0)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            return value.SafeGetLong(childPropertyName, defaultValue);
        }

        return defaultValue;
    }

    public static long SafeGetLong(
        this JsonElement input,
        string propertyName,
        string childPropertyName,
        string subChildPropertyName,
        long defaultValue = 0)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            if (value.TryGetProperty(childPropertyName, out var value2) == true)
            {
                return
                    value2.SafeGetLong(subChildPropertyName);
            }
        }

        return defaultValue;
    }

    public static bool SafeGetBool(
        this JsonElement input, string propertyName,
        bool defaultValue = false)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            if (value.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            else if (value.ValueKind == JsonValueKind.False)
            {
                return false;
            }
        }

        return defaultValue;
    }

    public static bool SafeGetBool(
        this JsonElement input,
        string propertyName,
        string childPropertyName,
        bool defaultValue = false)
    {
        if (input.TryGetProperty(propertyName, out var value) == true)
        {
            return value.SafeGetBool(childPropertyName, defaultValue);
        }

        return defaultValue;
    }

    #region JsonNode Extension Methods

    /// <summary>
    /// Gets a string value from a JsonNode property safely, returning empty string if not found.
    /// </summary>
    /// <param name="node">The JsonNode to search in.</param>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The property value as a string, or empty string if not found or null.</returns>
    public static string GetString(this JsonNode? node, string propertyName)
    {
        if (node == null)
        {
            return string.Empty;
        }
        else
        {
            var match = node[propertyName];

            if (match == null)
            {
                return string.Empty;
            }
            else
            {
                return match.ToString();
            }
        }
    }

    /// <summary>
    /// Gets an integer value from a JsonNode property safely, returning 0 if not found or cannot parse.
    /// </summary>
    /// <param name="node">The JsonNode to search in.</param>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>The property value as an integer, or 0 if not found, null, or cannot parse.</returns>
    public static int GetInt32(this JsonNode? node, string propertyName)
    {
        if (node == null)
        {
            return 0;
        }
        else
        {
            var match = node[propertyName];

            if (match == null)
            {
                return 0;
            }
            else
            {
                var valueAsString = match.ToString();

                if (int.TryParse(valueAsString, out int result) == true)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    /// <summary>
    /// Finds the first item in a JsonArray that has the specified property name.
    /// </summary>
    /// <param name="array">The JsonArray to search in.</param>
    /// <param name="propertyName">The name of the property to look for.</param>
    /// <returns>The value of the property from the first matching item, or null if not found.</returns>
    public static JsonNode? FirstOrDefaultWithPropertyName(
        this JsonArray? array,
        string propertyName)
    {
        if (array == null)
        {
            return null;
        }

        foreach (var item in array)
        {
            if (item == null)
            {
                continue;
            }
            else if (item[propertyName] != null)
            {
                return item[propertyName];
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a JsonArray property from a JsonNode.
    /// </summary>
    /// <param name="node">The JsonNode to search in.</param>
    /// <param name="propertyName">The name of the array property to retrieve.</param>
    /// <returns>The JsonArray if found and is an array, otherwise null.</returns>
    public static JsonArray? GetArray(
        this JsonNode? node,
        string propertyName)
    {
        if (node == null)
        {
            return null;
        }

        // get reference to array property
        var array = node[propertyName];

        if (array == null)
        {
            return null;
        }
        else
        {
            if (array is JsonArray valueAsArray)
            {
                return valueAsArray;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Gets a specific item from a JsonArray by searching for a property name/value match.
    /// </summary>
    /// <param name="node">The JsonNode containing the array.</param>
    /// <param name="arrayPropertyName">The name of the array property.</param>
    /// <param name="searchPropertyName">The property name to search for within array items.</param>
    /// <param name="searchPropertyValue">The property value to match.</param>
    /// <returns>The matching JsonNode item, or null if not found.</returns>
    public static JsonNode? GetArrayItem(
        this JsonNode? node,
        string arrayPropertyName,
        string searchPropertyName, string searchPropertyValue)
    {
        if (node == null)
        {
            return null;
        }

        var array = node.GetArray(arrayPropertyName);

        if (array == null)
        {
            return null;
        }
        else
        {
            foreach (var item in array)
            {
                if (item == null)
                {
                    continue;
                }
                else if (item[searchPropertyName] != null &&
                    item[searchPropertyName]!.ToString() == searchPropertyValue)
                {
                    return item;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Safely gets a specific item from a JsonArray by searching for a property name/value match.
    /// This method provides a safe way to retrieve array items without throwing exceptions.
    /// </summary>
    /// <param name="array">The JsonArray to search in.</param>
    /// <param name="searchPropertyName">The property name to search for within array items.</param>
    /// <param name="searchPropertyValue">The property value to match.</param>
    /// <returns>The matching JsonNode item, or null if not found.</returns>
    public static JsonNode? SafeGetArrayItem(
        this JsonArray? array,
        string searchPropertyName,
        string searchPropertyValue)
    {
        if (array == null)
        {
            return null;
        }

        foreach (var item in array)
        {
            if (item == null)
            {
                continue;
            }
            else if (item[searchPropertyName] != null &&
                item[searchPropertyName]!.ToString() == searchPropertyValue)
            {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Converts a JsonArray containing objects with key/value properties into a Dictionary.
    /// </summary>
    /// <param name="array">The JsonArray to convert.</param>
    /// <param name="keyPropertyName">The property name to use as dictionary keys.</param>
    /// <param name="valuePropertyName">The property name to use as dictionary values.</param>
    /// <returns>A dictionary with string keys and values, or an empty dictionary if array is null.</returns>
    public static Dictionary<string, string> GetDictionary(
        this JsonArray? array,
        string keyPropertyName,
        string valuePropertyName)
    {
        var dictionary = new Dictionary<string, string>();

        if (array == null)
        {
            return dictionary;
        }

        foreach (var item in array)
        {
            if (item == null)
            {
                continue;
            }

            var key = item.GetString(keyPropertyName);
            var value = item.GetString(valuePropertyName);

            if (!string.IsNullOrEmpty(key))
            {
                dictionary[key] = value;
            }
        }

        return dictionary;
    }

    /// <summary>
    /// Gets a JsonArray property from a JsonNode and converts it into a Dictionary.
    /// </summary>
    /// <param name="node">The JsonNode containing the array property.</param>
    /// <param name="arrayPropertyName">The name of the array property to retrieve.</param>
    /// <param name="keyPropertyName">The property name to use as dictionary keys.</param>
    /// <param name="valuePropertyName">The property name to use as dictionary values.</param>
    /// <returns>A dictionary with string keys and values, or an empty dictionary if array is not found.</returns>
    public static Dictionary<string, string> GetDictionary(
        this JsonNode? node,
        string arrayPropertyName,
        string keyPropertyName,
        string valuePropertyName)
    {
        var array = node.GetArray(arrayPropertyName);
        return array.GetDictionary(keyPropertyName, valuePropertyName);
    }

    #endregion
}
