using System;

namespace Core.FileSystem {
    public class FsNodeChangedEventArgs : EventArgs {
        public FsNodeChangedEventArgs(IFsNode fsNode) {
            FsNode = fsNode;
        }

        public IFsNode FsNode { get; }
    }
}
