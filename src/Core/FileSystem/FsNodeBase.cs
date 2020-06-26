using System.IO.Abstractions;

namespace Core.FileSystem
{
    public abstract class FsNodeBase<T> : IFsNode where T : IFileSystemInfo
    {
        protected FsNodeBase(T info)
        {
            Info = info;
        }

        protected T Info { get; }

        public string Name => Info.Name;

        public string FullName => Info.FullName;

        public bool Exists => Info.Exists;

        public abstract void Rename(string newName);

        public override string ToString()
        {
            return Info.Name;
        }
    }
}
