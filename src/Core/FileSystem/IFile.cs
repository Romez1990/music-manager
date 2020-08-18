namespace Core.FileSystem
{
    public interface IFile : IFsNode<IFile>
    {
        string Extension { get; }
    }
}
