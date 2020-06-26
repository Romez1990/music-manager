using System;

namespace Core.FileSystem
{
    public interface IFsNodeElement
    {
        IFsNode FsNode { get; }

        CheckState CheckState { get; }

        event EventHandler<FsNodeElementCheckEventArgs> CheckEvent;

        event EventHandler<FsNodeElementCheckEventArgs> UncheckEvent;

        void Check();

        void Uncheck();
    }
}
