using System;
using Core.FileSystem;

namespace Core.FileSystemElement {
    public interface IFsNodeElement : IFsNodeBase {
        CheckState CheckState { get; }
        event EventHandler<FsNodeElementChangedEventArgs> Changed;
        void Unsubscribe();
        T Match<T>(Func<IDirectoryElement, T> onDirectory, Func<IFileElement, T> onFile);
        IFsNodeElement MatchDirectory(Func<IDirectoryElement, IFsNodeElement> onDirectory);
        IFsNodeElement MatchFile(Func<IFileElement, IFsNodeElement> onFile);
    }
}
