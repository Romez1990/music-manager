namespace Core.FileSystem
{
    public interface IFile : IFsNode
    {
        string Extension { get; }

        IFile Rename(string newName);
    }
}
