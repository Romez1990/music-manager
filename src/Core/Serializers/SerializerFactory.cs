using System;
using System.Collections.Generic;
using System.Linq;
using Core.IocContainer;
using Utils.Reflection;

namespace Core.Serializers {
    [Service]
    public class SerializerFactory : ISerializerFactory {
        public SerializerFactory() {
            _serializers = CreateSerializers();
        }

        private readonly IReadOnlyList<IReadOnlyList<Lazy<ISerializer>>> _serializers;

        private IReadOnlyList<IReadOnlyList<Lazy<ISerializer>>> CreateSerializers() =>
            Enum.GetValues<Format>()
                .Map(CreateSerializersByFormat)
                .ToArray();

        private IReadOnlyList<Lazy<ISerializer>> CreateSerializersByFormat(Format format) =>
            Enum.GetValues<NamingConvention>()
                .Map(namingConvention => CreateSerializer(format, namingConvention))
                .ToArray();

        private readonly IReadOnlyDictionary<Format, Type> _serializerTypes =
            new Dictionary<Format, Type> {
                { Format.Json, typeof(JsonSerializer) },
                { Format.Yaml, typeof(YamlSerializer) },
            };

        private Lazy<ISerializer> CreateSerializer(Format format, NamingConvention namingConvention) =>
            new(() => {
                var serializerType = _serializerTypes[format];
                return serializerType.Construct<ISerializer>(new object[] { namingConvention });
            });

        public ISerializer GetSerializer(Format format, NamingConvention namingConvention) =>
            _serializers[(int)format][(int)namingConvention].Value;
    }
}
