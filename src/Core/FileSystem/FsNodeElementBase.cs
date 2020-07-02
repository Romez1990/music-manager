using System;

namespace Core.FileSystem
{
    public abstract class FsNodeElementBase<T> : IFsNodeElement<T> where T : IFsNode
    {
        protected FsNodeElementBase(T fsNode, EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler,
            CheckState checkState) : this(fsNode, checkState)
        {
            CheckStateChange += checkStateChangeHandler;
            CheckStateChangeHandler = checkStateChangeHandler;
        }

        protected FsNodeElementBase(T fsNode, EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler) :
            this(fsNode)
        {
            CheckStateChange += checkStateChangeHandler;
            CheckStateChangeHandler = checkStateChangeHandler;
        }

        private FsNodeElementBase(T fsNode, CheckState checkState)
        {
            FsNode = fsNode;
            CheckState = checkState;
        }

        private FsNodeElementBase(T fsNode)
        {
            FsNode = fsNode;
            CheckState = CheckState.Unchecked;
        }

        public T FsNode { get; }

        protected event EventHandler<FsNodeElementCheckEventArgs> CheckStateChange;

        protected readonly EventHandler<FsNodeElementCheckEventArgs> CheckStateChangeHandler;

        protected void OnCheckStateChange(IFsNodeElement<IFsNode> newFsNodeElement)
        {
            CheckStateChange?.Invoke(this, new FsNodeElementCheckEventArgs(newFsNodeElement));
        }

        public CheckState CheckState { get; }

        public abstract void Uncheck();

        public abstract void Check();

        public override string ToString()
        {
            return FsNode.ToString();
        }
    }
}
