namespace Core.Configuration
{
    public class ConfigurationKeyNotFoundException : ConfigurationException
    {
        public ConfigurationKeyNotFoundException(string key) : base($"Key {key} not found")
        {
        }
    }
}
