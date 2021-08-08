using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.HttpClient.Exceptions;
using Core.IocContainer;
using Core.Serializers;
using LanguageExt;
using static LanguageExt.Prelude;

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
            IReadOnlyDictionary<string, string> parameters = null) {
            var castException = new Func<RequestException, RequestException>(identity);
            return SendRequest(GetUrl(url, parameters))
                .ToAsync()
                .Bind(response => ResponseToEither(response).MapLeft(castException).ToAsync())
                .MapAsync(response => response.Content.ReadAsStringAsync());
        }

        public EitherAsync<RequestException, T> Get<T>(string url,
            IReadOnlyDictionary<string, string> parameters = null) =>
            GetText(url, parameters)
                .Map(_serializer.Deserialize<T>);

        private async Task<Either<RequestException, HttpResponseMessage>> SendRequest(string url) {
            try {
                return await _httpClient.GetAsync(url);
            } catch (HttpRequestException exception) {
                return new UnknownRequestException(exception);
            }
        }

        private string GetUrl(string baseUrl, IReadOnlyDictionary<string, string> parametersOrNull) {
            if (parametersOrNull is null || !parametersOrNull.Any())
                return baseUrl;

            var keyValues = parametersOrNull.Map(pair => $"{pair.Key}={WebUtility.UrlEncode(pair.Value)}");
            var stringParameters = string.Join('&', keyValues);
            return $"{baseUrl}?{stringParameters}";
        }

        private Either<FailedRequestException, HttpResponseMessage> ResponseToEither(HttpResponseMessage response) {
            if (response.IsSuccessStatusCode)
                return response;
            return (int)response.StatusCode >= 500
                ? new ServerRequestException(response)
                : new ClientRequestException(response);
        }
    }
}
