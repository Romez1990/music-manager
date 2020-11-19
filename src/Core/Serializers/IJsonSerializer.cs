namespace Core.Serializers
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string json);
    }
}
