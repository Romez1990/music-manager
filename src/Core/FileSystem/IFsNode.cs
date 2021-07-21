using System;

namespace Core.FileSystem {
    public interface IFsNode : IFsNodeBase {
        event EventHandler<FsNodeChangedEventArgs> Changed;
        void Unsubscribe();
        T Match<T>(Func<IDirectory, T> onDirectory, Func<IFile, T> onFile);
        IFsNode MatchDirectory(Func<IDirectory, IFsNode> onDirectory);
        IFsNode MatchFile(Func<IFile, IFsNode> onFile);
    }
}
