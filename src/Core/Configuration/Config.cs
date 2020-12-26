#nullable enable
namespace Core.Configuration
{
    public class Config : IConfig
    {
        public Config(IConfigDriver configDriver)
        {
            _configDriver = configDriver;
        }

        private readonly IConfigDriver _configDriver;

        public string GetString(string key)
        {
            var value = _configDriver.GetString(key);
            if (value is null)
                throw new ConfigurationKeyNotFoundException(key);
            return value;
        }
    }
}
