namespace Core.FileSystem
{
    public interface IFileElement : IFsNodeElement<IFileElement>
    {
        string Extension { get; }
    }
}
