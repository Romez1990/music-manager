using Newtonsoft.Json;

namespace Core.Serializers
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T value) =>
            JsonConvert.SerializeObject(value);

        public T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json);
    }
}
