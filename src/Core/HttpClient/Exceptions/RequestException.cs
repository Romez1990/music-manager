using System;

namespace Core.HttpClient.Exceptions {
    public abstract class RequestException : Exception {
        protected RequestException(string message) : base(message) { }
    }
}
