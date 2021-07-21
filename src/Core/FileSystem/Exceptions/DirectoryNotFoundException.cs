namespace Core.FileSystem.Exceptions {
    public sealed class DirectoryNotFoundException : FsNodeNotFoundException {
        public DirectoryNotFoundException(string directoryName) : base("directory", directoryName) { }
    }
}
