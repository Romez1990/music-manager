using System.Configuration;

namespace Core.Configuration
{
    public class Config : IConfig
    {
        public string GetString(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            if (value == null)
                throw new ConfigurationKeyNotFoundException(key);
            return value;
        }
    }
}
