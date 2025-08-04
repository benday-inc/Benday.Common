namespace Benday.Common.UnitTests.MockingUtilities;

public class ClassWithOneDependencyAndValidationLogicInTheConstructor
{
    public ClassWithOneDependencyAndValidationLogicInTheConstructor(
        ISampleConfigurationInfo configurationInfo)
    {
        ConfigurationInfo = configurationInfo;

        if (string.IsNullOrWhiteSpace(configurationInfo.ConfigurationValue))
        {
            throw new ArgumentException(
                "Configuration value cannot be null or empty.",
                nameof(configurationInfo));
        }
    }

    public ISampleConfigurationInfo ConfigurationInfo { get; private set; }

}

