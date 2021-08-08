using System.Net.Http;

namespace Core.HttpClient.Exceptions {
    public sealed class ServerRequestException : FailedRequestException {
        public ServerRequestException(HttpResponseMessage response) : base(response) { }
    }
}
