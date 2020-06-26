namespace Core.FileSystem
{
    public interface IFsNodeElement<out T> where T : IFsNode
    {
        T FsNode { get; }

        CheckState CheckState { get; }

        void Check();

        void Uncheck();
    }
}
