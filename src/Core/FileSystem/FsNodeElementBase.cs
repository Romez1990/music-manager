using System;

namespace Core.FileSystem
{
    public abstract class FsNodeElementBase<T> : IFsNodeElement where T : IFsNode
    {
        protected FsNodeElementBase(T fsNode, EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler) : this(fsNode)
        {
            UncheckEvent += uncheckHandler;
            CheckEvent += checkHandler;
        }

        protected FsNodeElementBase(T fsNode)
        {
            FsNode = fsNode;
            CheckState = CheckState.Unchecked;
        }

        protected T FsNode { get; }

        public string Name => FsNode.Name;

        public string Path => FsNode.Path;

        public abstract void Rename(string newName);

        protected event EventHandler<FsNodeElementCheckEventArgs> UncheckEvent;

        protected event EventHandler<FsNodeElementCheckEventArgs> CheckEvent;

        protected void OnUncheckEvent()
        {
            UncheckEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        protected void OnCheckEvent()
        {
            CheckEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        public CheckState CheckState { get; protected set; }

        public virtual void Uncheck()
        {
            CheckState = CheckState.Unchecked;
            OnUncheckEvent();
        }

        public virtual void Check()
        {
            CheckState = CheckState.Checked;
            OnCheckEvent();
        }

        public override string ToString()
        {
            return FsNode.ToString();
        }
    }
}
