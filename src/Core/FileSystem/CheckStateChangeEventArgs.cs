using System;

namespace Core.FileSystem
{
    public class CheckStateChangeEventArgs : EventArgs
    {
        public CheckStateChangeEventArgs(IFsNodeElement<object> fsNodeElement)
        {
            FsNodeElement = fsNodeElement;
        }

        public IFsNodeElement<object> FsNodeElement { get; }
    }
}
