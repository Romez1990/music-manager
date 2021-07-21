namespace Core.FileSystem.Exceptions {
    public sealed class DirectoryAccessDeniedException : FsNodeAccessDeniedException {
        public DirectoryAccessDeniedException(string directoryName) : base("directory", directoryName) { }
    }
}
