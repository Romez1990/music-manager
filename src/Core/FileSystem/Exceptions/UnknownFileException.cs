namespace Core.FileSystem.Exceptions {
    public sealed class UnknownFileException : FsException {
        public UnknownFileException(string message) : base(message) { }
    }
}
