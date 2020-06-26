namespace Core.FileSystem
{
    public interface IFsNode
    {
        string Name { get; }

        string FullName { get; }

        bool Exists { get; }

        void Rename(string newName);
    }
}
