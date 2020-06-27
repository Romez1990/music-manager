using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler) : base(directory, uncheckHandler, checkHandler)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = GetContent();
        }

        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory) : base(directory)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = GetContent();
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public ImmutableArray<IFsNodeElement<IFsNode>> Content { get; }

        private ImmutableArray<IFsNodeElement<IFsNode>> GetContent()
        {
            return FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory =>
                    _fsNodeElementFactory.CreateDirectoryElementInsideDirectory(directory, null, null, null))
                .Cast<IFsNodeElement<IFsNode>>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file => _fsNodeElementFactory.CreateFileElementInsideDirectory(file, null, null)))
                .ToImmutableArray();
        }
    }
}
