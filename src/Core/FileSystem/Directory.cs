using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Linq;
using IO = System.IO;

namespace Core.FileSystem
{
    public class Directory : FsNodeBase<IDirectoryInfo>, IDirectory
    {
        public Directory(IFsNodeFactory fsNodeFactory, IFsInfoFactory fsInfoFactory, IDirectoryInfo info) : base(info)
        {
            _fsNodeFactory = fsNodeFactory;
            _fsInfoFactory = fsInfoFactory;
            Content = ReadContent();
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        private readonly IFsInfoFactory _fsInfoFactory;

        public ImmutableArray<IFsNode> Content { get; }

        private ImmutableArray<IFsNode> ReadContent()
        {
            var directories = Info
                .GetDirectories()
                .Select(info => _fsNodeFactory.InstantiateDirectory(info))
                .Cast<IFsNode>()
                .OrderBy(fsNode => fsNode.Name);
            var files = Info
                .GetFiles()
                .Select(info => _fsNodeFactory.InstantiateFile(info))
                .OrderBy(fsNode => fsNode.Name);
            return directories
                .Concat(files)
                .ToImmutableArray();
        }

        public IDirectory Rename(string newName)
        {
            var parentPath = Info.Parent.FullName;
            var newPath = IO.Path.Combine(parentPath, newName);
            IO.Directory.Move(Path, newPath);
            return new Directory(_fsNodeFactory, _fsInfoFactory, _fsInfoFactory.CreateDirectoryInfo(newPath));
        }
    }
}
