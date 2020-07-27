using System;

namespace Core.FileSystem
{
    public abstract class FsNodeElementBase<TThis, TFsNode> : IFsNodeElement<TThis>
        where TFsNode : IFsNode<object>
    {
        protected FsNodeElementBase(TFsNode fsNode, EventHandler<CheckStateChangeEventArgs> checkStateChange,
            CheckState checkState)
        {
            FsNode = fsNode;
            CheckStateChange += checkStateChange;
            CheckStateChangeHandler = checkStateChange;
            CheckState = checkState;
        }

        protected TFsNode FsNode { get; }

        public string Name => FsNode.Name;

        public string Path => FsNode.Path;

        public abstract TThis Rename(string newName);

        protected event EventHandler<CheckStateChangeEventArgs> CheckStateChange;

        protected readonly EventHandler<CheckStateChangeEventArgs> CheckStateChangeHandler;

        protected void OnCheckStateChangeEvent(IFsNodeElement<object> newFsNodeElement)
        {
            CheckStateChange?.Invoke(this, new CheckStateChangeEventArgs(newFsNodeElement));
        }

        /*
        protected void OnCheckStateChangeEvent(IFsNodeElement<object> sender, IFsNodeElement<object> newFsNodeElement)
        {
            CheckStateChangeHandler?.Invoke(sender, new CheckStateChangeEventArgs(newFsNodeElement));
        }
        */

        public CheckState CheckState { get; }

        public abstract TThis Uncheck();

        public abstract TThis Check();

        public override string ToString()
        {
            return FsNode.ToString();
        }
    }
}
