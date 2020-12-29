namespace Core.Configuration
{
    public class BoolConfigurationKeyException : ConfigurationException
    {
        public BoolConfigurationKeyException(string key) : base($"environment variable {key} must be true or false")
        {
        }
    }
}
