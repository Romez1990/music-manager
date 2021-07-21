namespace Core.FileSystem.Exceptions {
    public sealed class DirectoryIsBeingUsedException : FsNodeIsBeingUsedException {
        public DirectoryIsBeingUsedException(string directoryName) : base("directory", directoryName) { }
    }
}
