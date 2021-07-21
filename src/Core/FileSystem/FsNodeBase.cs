using System;
using System.IO;
using static LanguageExt.Prelude;

namespace Core.FileSystem {
    public abstract class FsNodeBase<T> : IFsNode where T : FileSystemInfo {
        protected FsNodeBase(T info) {
            Info = info;
        }

        protected T Info { get; }

        public bool Exists => Info.Exists;
        public string Name => Info.Name;
        public string Path => Info.FullName;
        public abstract string ParentPath { get; }

        public event EventHandler<FsNodeChangedEventArgs> Changed;

        protected void InvokeChanged(IFsNode fsNode) =>
            Changed?.Invoke(this, new FsNodeChangedEventArgs(fsNode));

        public void Unsubscribe() =>
            Changed = null;

        public T1 Match<T1>(Func<IDirectory, T1> onDirectory, Func<IFile, T1> onFile) =>
            this switch {
                IDirectory directory => onDirectory(directory),
                IFile file => onFile(file),
                _ => throw new NotSupportedException($"Type {GetType()} is not supported"),
            };

        public IFsNode MatchDirectory(Func<IDirectory, IFsNode> onDirectory) =>
            Match(onDirectory, identity);

        public IFsNode MatchFile(Func<IFile, IFsNode> onFile) =>
            Match(identity, onFile);
    }
}
