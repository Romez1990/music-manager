using Newtonsoft.Json;

namespace Core.Serializers
{
    public class JsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string json) =>
            JsonConvert.DeserializeObject<T>(json);
    }
}
