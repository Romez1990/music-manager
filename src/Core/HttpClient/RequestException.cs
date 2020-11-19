using System;
using System.Net.Http;

namespace Core.HttpClient
{
    public class RequestException : Exception
    {
        public RequestException(HttpResponseMessage response)
            : base($"Request fail with status code {GetStatusCode(response)}")
        {
            StatusCode = GetStatusCode(response);
        }

        public int StatusCode { get; }

        private static int GetStatusCode(HttpResponseMessage response) =>
            (int)response.StatusCode;
    }
}
