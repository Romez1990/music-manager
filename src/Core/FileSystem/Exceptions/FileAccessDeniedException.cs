namespace Core.FileSystem.Exceptions {
    public sealed class FileAccessDeniedException : FsNodeAccessDeniedException {
        public FileAccessDeniedException(string fileName) : base("file", fileName) { }
    }
}
