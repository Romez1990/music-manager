namespace Core.FileSystem
{
    public interface IFileElement : IFsNodeElement
    {
        string Extension { get; }
    }
}
