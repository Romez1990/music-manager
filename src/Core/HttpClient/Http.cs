#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
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

        public EitherAsync<RequestException, T> Get<T>(string url, IDictionary<string, string>? parameters = null) =>
            GetText(url, parameters)
                .Map(_jsonSerializer.Deserialize<T>);

        private EitherAsync<RequestException, string> GetText(string url,
            IDictionary<string, string>? parameters = null) =>
            _httpClient.GetAsync(GetUrl(url, parameters))
                .Map(CatchException)
                .ToAsync()
                .MapAsync(response => response.Content.ReadAsStringAsync());

        private string GetUrl(string baseUrl, IDictionary<string, string>? parameters)
        {
            if (parameters == null) return baseUrl;

            var builder = new UriBuilder(baseUrl)
            {
                Port = -1,
            };
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var (key, value) in parameters)
                query[key] = value;
            builder.Query = query.ToString();
            return builder.ToString();
        }

        private Either<RequestException, HttpResponseMessage> CatchException(HttpResponseMessage response) =>
            response.IsSuccessStatusCode
                ? (Either<RequestException, HttpResponseMessage>)response
                : new RequestException(response);
    }
}
