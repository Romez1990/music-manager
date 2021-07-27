using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Serializers {
    public class JsonSerializer : ISerializer {
        public JsonSerializer(NamingConvention namingConvention) {
            _settings = new Lazy<JsonSerializerSettings>(() => {
                var contractResolver = new DefaultContractResolver {
                    NamingStrategy = GetNamingStrategy(namingConvention),
                };
                return new JsonSerializerSettings {
                    ContractResolver = contractResolver,
                };
            });
        }

        public string FileExtension => ".json";

        private readonly Lazy<JsonSerializerSettings> _settings;

        private NamingStrategy GetNamingStrategy(NamingConvention namingConvention) =>
            namingConvention switch {
                NamingConvention.SnakeCase => new SnakeCaseNamingStrategy(),
                NamingConvention.CamelCase => new CamelCaseNamingStrategy(),
                NamingConvention.PascalCase => new DefaultNamingStrategy(),
                NamingConvention.KebabCase => new KebabCaseNamingStrategy(),
                _ => throw new NotSupportedException(),
            };

        public string Serialize<T>(T value) =>
            JsonConvert.SerializeObject(value, _settings.Value);

        public T Deserialize<T>(string input) =>
            JsonConvert.DeserializeObject<T>(input, _settings.Value);
    }
}
