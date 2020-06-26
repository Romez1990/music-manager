namespace Core.FileSystem
{
    public interface IFile : IFsNode
    {
        string DirectoryName { get; }

        string Extension { get; }
    }
}
