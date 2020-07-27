namespace Core.FileSystem
{
    public interface IFsNode<out TThis>
    {
        string Name { get; }
        string Path { get; }
        TThis Rename(string newName);
    }
}
