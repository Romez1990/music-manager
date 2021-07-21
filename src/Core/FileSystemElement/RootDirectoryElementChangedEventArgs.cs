using System;

namespace Core.FileSystemElement {
    public class RootDirectoryElementChangedEventArgs : EventArgs {
        public RootDirectoryElementChangedEventArgs(IDirectoryElement directory) {
            Directory = directory;
        }

        public IDirectoryElement Directory { get; }
    }
}
