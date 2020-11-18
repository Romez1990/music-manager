namespace Core.FileSystem
{
    public abstract class FsNodeElementBase<TThis, TFsNode> : IFsNodeElement<TThis>
        where TFsNode : IFsNode<object>
    {
        protected FsNodeElementBase(TFsNode fsNode, CheckState checkState)
        {
            FsNode = fsNode;
            CheckState = checkState;
        }

        protected TFsNode FsNode { get; }

        public string Name => FsNode.Name;

        public string Path => FsNode.Path;

        public abstract TThis Rename(string newName);

        public CheckState CheckState { get; }

        public abstract TThis Uncheck();

        public abstract TThis Check();

        public override string ToString() => FsNode.ToString();
    }
}
