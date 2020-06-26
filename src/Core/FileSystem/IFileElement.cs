namespace Core.FileSystem
{
    public interface IFileElement : IFsNodeElement
    {
        new IFile FsNode { get; }
    }
}
