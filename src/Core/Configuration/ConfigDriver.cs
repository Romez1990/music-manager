#nullable enable
using System.Configuration;

namespace Core.Configuration
{
    public class ConfigDriver : IConfigDriver
    {
        public string? GetString(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
