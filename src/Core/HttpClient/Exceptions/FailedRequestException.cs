using System.Net.Http;

namespace Core.HttpClient.Exceptions {
    public abstract class FailedRequestException : RequestException {
        protected FailedRequestException(HttpResponseMessage response) : base(GetMessage(response)) {
            StatusCode = GetStatusCode(response);
            // TODO: save response body
        }

        public int StatusCode { get; }

        private static string GetMessage(HttpResponseMessage response) =>
            $"Request failed with status code {GetStatusCode(response)} {GetStatusCodeRepresentation(response)}";

        private static int GetStatusCode(HttpResponseMessage response) =>
            (int)response.StatusCode;

        private static string GetStatusCodeRepresentation(HttpResponseMessage response) =>
            response.StatusCode.ToString();
    }
}
