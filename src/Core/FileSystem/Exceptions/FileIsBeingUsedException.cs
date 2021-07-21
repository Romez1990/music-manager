namespace Core.FileSystem.Exceptions {
    public sealed class FileIsBeingUsedException : FsNodeIsBeingUsedException {
        public FileIsBeingUsedException(string fileName) : base("file", fileName) { }
    }
}
