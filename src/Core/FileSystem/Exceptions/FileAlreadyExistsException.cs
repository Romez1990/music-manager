namespace Core.FileSystem.Exceptions {
    public sealed class FileAlreadyExistsException : FsNodeAlreadyExistsException {
        public FileAlreadyExistsException(string newFileName) : base("file", newFileName) { }
    }
}
