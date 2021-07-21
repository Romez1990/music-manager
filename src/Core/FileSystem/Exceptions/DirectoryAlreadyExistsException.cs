namespace Core.FileSystem.Exceptions {
    public sealed class DirectoryAlreadyExistsException : FsNodeAlreadyExistsException {
        public DirectoryAlreadyExistsException(string newDirectoryName) : base("directory", newDirectoryName) { }
    }
}
