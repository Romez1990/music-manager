using System;
using Core.FileSystem;
using static LanguageExt.Prelude;

namespace Core.FileSystemElement {
    public abstract class FsNodeElementBase<T> : IFsNodeElement where T : IFsNode {
        protected FsNodeElementBase(T fsNode, CheckState checkState) {
            FsNode = fsNode;
            CheckState = checkState;
        }

        protected T FsNode { get; }

        public bool Exists => FsNode.Exists;
        public string Name => FsNode.Name;
        public string Path => FsNode.Path;
        public string ParentPath => FsNode.ParentPath;

        public CheckState CheckState { get; }

        public event EventHandler<FsNodeElementChangedEventArgs> Changed;

        protected void InvokeChanged(IFsNodeElement fsNodeElement) =>
            Changed?.Invoke(this, new FsNodeElementChangedEventArgs(fsNodeElement));

        public void Unsubscribe() =>
            Changed = null;

        public T1 Match<T1>(Func<IDirectoryElement, T1> onDirectory, Func<IFileElement, T1> onFile) =>
            this switch {
                IDirectoryElement directory => onDirectory(directory),
                IFileElement file => onFile(file),
                _ => throw new NotSupportedException(),
            };

        public IFsNodeElement MatchDirectory(Func<IDirectoryElement, IFsNodeElement> onDirectory) =>
            Match(onDirectory, identity);

        public IFsNodeElement MatchFile(Func<IFileElement, IFsNodeElement> onFile) =>
            Match(identity, onFile);
    }
}
