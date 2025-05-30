using Microsoft.Extensions.Configuration;

namespace Benday.Common;

public static class ConfigurationExtensionMethods
{
    /// <summary>
    /// This method retrieves a value from the configuration and returns it as a string. If the key does not exist or the value is null, it returns the default value.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string SafeGetString(this IConfiguration config, string key, string defaultValue = "")
    {
        if (config == null)
        {
            return defaultValue;
        }

        if (config[key] == null)
        {
            return defaultValue;
        }

        var temp = config[key].SafeToString(defaultValue);

        return temp;
    }

    /// <summary>
    /// This method retrieves a value from the configuration and returns it as a string. If the key does not exist or the value is null or empty, it throws an exception.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetStringThrowIfNullOrEmpty(this IConfiguration config, string key)
    {
        var temp = config[key].ToStringThrowIfNullOrEmpty(key);

        return temp;
    }
}