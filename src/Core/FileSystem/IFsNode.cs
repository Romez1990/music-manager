namespace Core.FileSystem
{
    public interface IFsNode
    {
        string Name { get; }
        string Path { get; }
        void Rename(string newName);
    }
}
