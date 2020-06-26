using System;

namespace Core.FileSystem
{
    public abstract class FsNodeElementBase<T> : IFsNodeElement<T> where T : IFsNode
    {
        protected FsNodeElementBase(T fsNode, EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : this(fsNode)
        {
            CheckEvent += checkHandler;
            UncheckEvent += uncheckHandler;
        }

        protected FsNodeElementBase(T fsNode)
        {
            FsNode = fsNode;
            CheckState = CheckState.Unchecked;
        }

        public T FsNode { get; }

        protected event EventHandler<FsNodeElementCheckEventArgs> CheckEvent;

        protected event EventHandler<FsNodeElementCheckEventArgs> UncheckEvent;

        protected void OnCheckEvent()
        {
            CheckEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        protected void OnUncheckEvent()
        {
            UncheckEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        public CheckState CheckState { get; protected set; }

        public virtual void Check()
        {
            CheckState = CheckState.Checked;
            OnCheckEvent();
        }

        public virtual void Uncheck()
        {
            CheckState = CheckState.Unchecked;
            OnUncheckEvent();
        }

        public override string ToString()
        {
            return FsNode.ToString();
        }
    }
}
