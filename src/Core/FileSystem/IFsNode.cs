using System;

namespace Core.FileSystem {
    public interface IFsNode : IFsNodeBase {
        event EventHandler<FsNodeChangedEventArgs> Changed;
        void Unsubscribe();
    }
}
