using System.IO.Abstractions;

namespace Core.FileSystem
{
    public abstract class FsNodeBase<TThis, TInfo> : IFsNode<TThis>
        where TInfo : IFileSystemInfo
    {
        protected FsNodeBase(TInfo info)
        {
            Info = info;
        }

        protected TInfo Info { get; }

        public string Name => Info.Name;

        public string Path => Info.FullName;

        public abstract TThis Rename(string newName);

        public override string ToString()
        {
            return Info.Name;
        }
    }
}
