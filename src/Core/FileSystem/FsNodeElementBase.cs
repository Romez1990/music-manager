using System;

namespace Core.FileSystem
{
    public abstract class FsNodeElementBase : IFsNodeElement
    {
        protected FsNodeElementBase(IFsNode fsNode, EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : this(fsNode)
        {
            CheckEvent += checkHandler;
            UncheckEvent += uncheckHandler;
        }

        protected FsNodeElementBase(IFsNode fsNode)
        {
            FsNode = fsNode;
        }

        public IFsNode FsNode { get; }

        public event EventHandler<FsNodeElementCheckEventArgs> CheckEvent;

        public event EventHandler<FsNodeElementCheckEventArgs> UncheckEvent;

        protected void OnCheckEvent()
        {
            CheckEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        protected void OnUncheckEvent()
        {
            UncheckEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        public CheckState CheckState { get; protected set; } = CheckState.Unchecked;

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
