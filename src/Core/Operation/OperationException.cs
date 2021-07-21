using System;

namespace Core.Operation {
    public sealed class OperationException : Exception {
        public OperationException(Exception innerException) : base(innerException.Message, innerException) { }
    }
}
