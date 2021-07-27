using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Core.Serializers {
    public class YamlSerializer : ISerializer {
        public YamlSerializer(NamingConvention namingConvention) {
            var convention = GetNamingConvention(namingConvention);
            _serializer = new Lazy<YamlDotNet.Serialization.ISerializer>(() => new SerializerBuilder()
                .WithNamingConvention(convention)
                .Build());
            _deserializer = new Lazy<IDeserializer>(() => new DeserializerBuilder()
                .WithNamingConvention(convention)
                .Build());
        }

        public string FileExtension => ".yml";

        private readonly Lazy<YamlDotNet.Serialization.ISerializer> _serializer;
        private readonly Lazy<IDeserializer> _deserializer;

        private INamingConvention GetNamingConvention(NamingConvention namingConvention) =>
            namingConvention switch {
                NamingConvention.SnakeCase => UnderscoredNamingConvention.Instance,
                NamingConvention.CamelCase => CamelCaseNamingConvention.Instance,
                NamingConvention.PascalCase => PascalCaseNamingConvention.Instance,
                NamingConvention.KebabCase => HyphenatedNamingConvention.Instance,
                _ => throw new NotSupportedException(),
            };

        public string Serialize<T>(T value) =>
            _serializer.Value.Serialize(value);

        public T Deserialize<T>(string input) =>
            _deserializer.Value.Deserialize<T>(input);
    }
}
