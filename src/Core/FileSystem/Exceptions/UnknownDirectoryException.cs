namespace Core.FileSystem.Exceptions {
    public sealed class UnknownDirectoryException : FsException {
        public UnknownDirectoryException(string message) : base(message) { }
    }
}
