namespace Core.FileSystem {
    public interface IFsNodeBase {
        bool Exists { get; }
        string Name { get; }
        string Path { get; }
        string ParentPath { get; }
    }
}
