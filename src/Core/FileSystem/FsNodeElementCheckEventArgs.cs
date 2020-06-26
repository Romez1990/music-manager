using System;

namespace Core.FileSystem
{
    public class FsNodeElementCheckEventArgs : EventArgs
    {
        public FsNodeElementCheckEventArgs(CheckState checkState)
        {
            CheckState = checkState;
        }

        public CheckState CheckState { get; }
    }
}
