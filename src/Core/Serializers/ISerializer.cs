namespace Core.Serializers {
    public interface ISerializer {
        string FileExtension { get; }
        string Serialize<T>(T value);
        T Deserialize<T>(string input);
    }
}
