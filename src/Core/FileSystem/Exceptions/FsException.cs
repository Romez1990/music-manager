using System;

namespace Core.FileSystem.Exceptions {
    public abstract class FsException : Exception {
        protected FsException(string message) : base(message) { }
    }
}
