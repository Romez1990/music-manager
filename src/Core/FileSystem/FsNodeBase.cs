using System;
using System.IO;

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
    }
}
