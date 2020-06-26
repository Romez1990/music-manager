using System.IO.Abstractions;

namespace Core.FileSystem
{
    public abstract class FsNodeBase<TInfo> : IFsNode where TInfo : IFileSystemInfo
    {
        protected FsNodeBase(TInfo info)
        {
            Info = info;
        }

        protected TInfo Info { get; }

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
