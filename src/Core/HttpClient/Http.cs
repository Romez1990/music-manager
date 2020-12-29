using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Core.Serializers;
using LanguageExt;

namespace Core.HttpClient
{
    public class Http : IHttp
    {
        public Http(IJsonSerializer jsonSerializer)
        {
            _httpClient = new System.Net.Http.HttpClient();
            _jsonSerializer = jsonSerializer;
        }

        private readonly System.Net.Http.HttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public EitherAsync<RequestException, string> Html(string url) =>
            GetText(url);

        public EitherAsync<RequestException, T> Get<T>(string url, IDictionary<string, string> parameters = null) =>
            GetText(url, parameters)
                .Map(_jsonSerializer.Deserialize<T>);

        private EitherAsync<RequestException, string> GetText(string url,
            IDictionary<string, string> parametersOrNull = null) =>
            _httpClient.GetAsync(GetUrl(url, parametersOrNull))
                .Map(CatchException)
                .ToAsync()
                .MapAsync(response => response.Content.ReadAsStringAsync());

        private string GetUrl(string baseUrl, IDictionary<string, string> parametersOrNull)
        {
            if (parametersOrNull is null || !parametersOrNull.Any()) return baseUrl;

            var keyValues = parametersOrNull.Map(pair => $"{pair.Key}={WebUtility.UrlEncode(pair.Value)}");
            var stringParameters = string.Join('&', keyValues.ToArray());
            return $"{baseUrl}?{stringParameters}";
        }

        private Either<RequestException, HttpResponseMessage> CatchException(HttpResponseMessage response) =>
            response.IsSuccessStatusCode
                ? (Either<RequestException, HttpResponseMessage>)response
                : new RequestException(response);
    }
}
