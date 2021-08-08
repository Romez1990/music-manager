using System.Net.Http;

namespace Core.HttpClient.Exceptions {
    public sealed class ClientRequestException : FailedRequestException {
        public ClientRequestException(HttpResponseMessage response) : base(response) { }
    }
}
