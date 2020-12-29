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
            var value = _configDriver.GetValueOrNull(key);
            if (value is null)
                throw new ConfigurationKeyNotFoundException(key);
            return value;
        }

        public bool GetBool(string key) =>
            GetString(key) switch
            {
                "true" => true,
                "false" => false,
                _ => throw new BoolConfigurationKeyException(key),
            };
    }
}
