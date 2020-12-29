using System.Configuration;

namespace Core.Configuration
{
    public class ConfigDriver : IConfigDriver
    {
        public string GetValueOrNull(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
