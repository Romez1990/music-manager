using System;

namespace Core.FileSystem {
    public interface IFsNode {
        bool Exists { get; }
        string Name { get; }
        string Path { get; }
        string ParentPath { get; }
        event EventHandler<FsNodeChangedEventArgs> Changed;
        void Unsubscribe();
    }
}
