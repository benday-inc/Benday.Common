using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
}
