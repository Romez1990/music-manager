namespace Core.FileSystem.Exceptions {
    public sealed class FileNotFoundException : FsNodeNotFoundException {
        public FileNotFoundException(string fileName) : base("file", fileName) { }
    }
}
