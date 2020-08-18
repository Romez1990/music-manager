namespace Core.FileSystem
{
    public interface IFsNodeElement<out TThis> : IFsNode<TThis>
    {
        CheckState CheckState { get; }
        TThis Uncheck();
        TThis Check();
    }
}
