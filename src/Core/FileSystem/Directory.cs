using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Linq;

namespace Core.FileSystem
{
    public class Directory : FsNodeBase<IDirectory, IDirectoryInfo>, IDirectory
    {
        public Directory(IFsInfoFactory fsInfoFactory, IFsNodeFactory fsNodeFactory, IDirectoryInfo info) : base(info)
        {
            _fsInfoFactory = fsInfoFactory;
            _fsNodeFactory = fsNodeFactory;
            Content = ReadContent();
        }

        private readonly IFsInfoFactory _fsInfoFactory;
        private readonly IFsNodeFactory _fsNodeFactory;

        public ImmutableArray<IFsNode<object>> Content { get; }

        private ImmutableArray<IFsNode<object>> ReadContent()
        {
            var directories = Info
                .GetDirectories()
                .Select(info => _fsNodeFactory.InstantiateDirectory(info))
                .Cast<IFsNode<object>>()
                .OrderBy(fsNode => fsNode.Name);
            var files = Info
                .GetFiles()
                .Select(info => _fsNodeFactory.InstantiateFile(info))
                .OrderBy(fsNode => fsNode.Name);
            return directories
                .Concat(files)
                .ToImmutableArray();
        }

        public override IDirectory Rename(string newName)
        {
            var newPath = System.IO.Path.Combine(Info.Parent.FullName, newName);
            var newInfo = _fsInfoFactory.CreateDirectoryInfo(Path);
            newInfo.MoveTo(newPath);
            return new Directory(_fsInfoFactory, _fsNodeFactory, newInfo);
        }
    }
}
