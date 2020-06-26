using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;

namespace Core.FileSystem
{
    public class Directory : FsNodeBase<IDirectoryInfo>, IDirectory
    {
        public Directory(IDirectoryInfo info) : base(info)
        {
        }

        public ImmutableArray<IFsNode> Content { get; }

        public override void Rename(string newName)
        {
            var newPath = Path.Combine(Info.Parent.FullName, newName);
            Info.MoveTo(newPath);
        }
    }
}
