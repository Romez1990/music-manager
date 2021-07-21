using System;

namespace Core.FileSystemElement {
    public class FsNodeElementChangedEventArgs : EventArgs {
        public FsNodeElementChangedEventArgs(IFsNodeElement fsNodeElement) {
            FsNodeElement = fsNodeElement;
        }

        public IFsNodeElement FsNodeElement { get; }
    }
}
