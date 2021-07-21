using System;

namespace Core.FileSystemElement.Exceptions {
    public sealed class CheckStateException : Exception {
        public CheckStateException(CheckState checkState) : base($"Check state is already set to {checkState}") { }
    }
}
