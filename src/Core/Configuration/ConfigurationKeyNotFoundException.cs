using System;

namespace Core.Configuration
{
    public class ConfigurationKeyNotFoundException : Exception
    {
        public ConfigurationKeyNotFoundException(string key) : base($"Key {key} not found")
        {
        }
    }
}
