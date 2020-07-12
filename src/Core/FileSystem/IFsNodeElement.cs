namespace Core.FileSystem
{
    public interface IFsNodeElement : IFsNode
    {
        CheckState CheckState { get; }
        void Uncheck();
        void Check();
    }
}
