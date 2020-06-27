using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Core.FileSystem
{
    public class Directory : FsNodeBase<IDirectoryInfo>, IDirectory
    {
        public Directory(IFsNodeFactory fsNodeFactory, IDirectoryInfo info) : base(info)
        {
            _fsNodeFactory = fsNodeFactory;
            Content = ReadContent();
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        public ImmutableArray<IFsNode> Content { get; }

        private ImmutableArray<IFsNode> ReadContent()
        {
            return Info
                .GetDirectories()
                .Select(info => _fsNodeFactory.InstantiateDirectory(info))
                .Cast<IFsNode>()
                .Concat(Info
                    .GetFiles()
                    .Select(info => _fsNodeFactory.InstantiateFile(info)))
                .ToImmutableArray();
        }

        public override void Rename(string newName)
        {
            var newPath = Path.Combine(Info.Parent.FullName, newName);
            Info.MoveTo(newPath);
        }
    }
}
