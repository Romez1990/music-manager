namespace Core.FileSystem
{
    public interface IFsNode
    {
        string Name { get; }
        string Path { get; }
        bool Exists { get; }
        void Rename(string newName);
    }
}
