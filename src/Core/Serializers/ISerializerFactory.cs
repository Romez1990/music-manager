namespace Core.Serializers {
    public interface ISerializerFactory {
        ISerializer GetSerializer(Format format, NamingConvention namingConvention);
    }
}
