using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Core.IocContainer;
using Core.Serializers;
using LanguageExt;

namespace Core.HttpClient {
    [Service]
    public class HttpClient : IHttpClient {
        public HttpClient(ISerializerFactory serializerFactory) {
            _httpClient = new System.Net.Http.HttpClient();
            _serializer = serializerFactory.GetSerializer(Format.Json, NamingConvention.CamelCase);
        }

        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly ISerializer _serializer;

        public EitherAsync<RequestException, string> GetText(string url,
            IReadOnlyDictionary<string, string> parameters = null) =>
            _httpClient.GetAsync(GetUrl(url, parameters))
                .Map(ResponseToEither)
                .ToAsync()
                .MapAsync(response => response.Content.ReadAsStringAsync());

        public EitherAsync<RequestException, T> Get<T>(string url,
            IReadOnlyDictionary<string, string> parameters = null) =>
            GetText(url, parameters)
                .Map(_serializer.Deserialize<T>);

        private string GetUrl(string baseUrl, IReadOnlyDictionary<string, string> parametersOrNull) {
            if (parametersOrNull is null || !parametersOrNull.Any())
                return baseUrl;

            var keyValues = parametersOrNull.Map(pair => $"{pair.Key}={WebUtility.UrlEncode(pair.Value)}");
            var stringParameters = string.Join('&', keyValues);
            return $"{baseUrl}?{stringParameters}";
        }

        private Either<RequestException, HttpResponseMessage> ResponseToEither(HttpResponseMessage response) =>
            response.IsSuccessStatusCode
                ? response
                : new RequestException(response);
    }
}
