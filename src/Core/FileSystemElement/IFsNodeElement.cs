using System;
using Core.FileSystem;

namespace Core.FileSystemElement {
    public interface IFsNodeElement : IFsNodeBase {
        CheckState CheckState { get; }
        event EventHandler<FsNodeElementChangedEventArgs> Changed;
        void Unsubscribe();
    }
}
