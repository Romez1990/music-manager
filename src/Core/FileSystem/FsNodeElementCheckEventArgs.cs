using System;

namespace Core.FileSystem
{
    public class FsNodeElementCheckEventArgs : EventArgs
    {
        public FsNodeElementCheckEventArgs(IFsNodeElement<IFsNode> fsNodeElement)
        {
            FsNodeElement = fsNodeElement;
        }

        public IFsNodeElement<IFsNode> FsNodeElement { get; }
    }
}
