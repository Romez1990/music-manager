using System;

namespace Core.HttpClient.Exceptions {
    public sealed class UnknownRequestException : RequestException {
        public UnknownRequestException(Exception exception) : base(exception.Message) { }
    }
}
