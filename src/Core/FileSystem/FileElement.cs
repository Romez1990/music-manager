namespace Core.FileSystem
{
    public class FileElement : FsNodeElementBase<IFileElement, IFile>, IFileElement
    {
        public FileElement(IFile file, CheckState checkState = CheckState.Unchecked) : base(file, checkState)
        {
        }

        public string Extension => FsNode.Extension;

        public override IFileElement Rename(string newName)
        {
            var newFile = FsNode.Rename(newName);
            return new FileElement(newFile, CheckState);
        }

        public override IFileElement Uncheck() =>
            new FileElement(FsNode);

        public override IFileElement Check() =>
            new FileElement(FsNode, CheckState.Checked);
    }
}
