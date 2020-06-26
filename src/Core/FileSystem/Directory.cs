using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Linq;
using Path = System.IO.Path;

namespace Core.FileSystem
{
    public class Directory : FsNodeBase<IDirectoryInfo>, IDirectory
    {
        public Directory(IFileSystemInfoFactory fileSystemInfoFactory, string path) :
            base(fileSystemInfoFactory.CreateDirectoryInfo(path))
        {
            Content = ReadContent();
        }

        public Directory(IDirectoryInfo info) : base(info)
        {
            Content = ReadContent();
        }

        public override void Rename(string newName)
        {
            var newPath = Path.Combine(Info.Parent.FullName, newName);
            Info.MoveTo(newPath);
        }

        public IDirectory Parent => new Directory(Info.Parent);

        public ImmutableArray<IFsNode> Content { get; }

        private ImmutableArray<IFsNode> ReadContent()
        {
            return Info
                .GetDirectories()
                .Select(info => new Directory(info))
                .Cast<IFsNode>()
                .Concat(Info
                    .GetFiles()
                    .Select(info => new File(info)))
                .ToImmutableArray();
        }
    }
}
