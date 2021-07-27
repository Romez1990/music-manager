using System;
using System.Net.Http;

namespace Core.HttpClient {
    public sealed class RequestException : Exception {
        public RequestException(HttpResponseMessage response) : base(GetMessage(response)) {
            StatusCode = GetStatusCode(response);
            // TODO: save response body
        }

        private static string GetMessage(HttpResponseMessage response) {
            return $"Request failed with status code {GetStatusCode(response)} {GetStatusCodeRepresentation(response)}";
        }

        public int StatusCode { get; }

        private static int GetStatusCode(HttpResponseMessage response) =>
            (int)response.StatusCode;

        private static string GetStatusCodeRepresentation(HttpResponseMessage response) =>
            response.StatusCode.ToString();
    }
}
