using System;
using System.Collections.Immutable;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IDirectory directory, EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : base(directory, checkHandler, uncheckHandler)
        {
        }

        public DirectoryElement(IDirectory directory) : base(directory)
        {
        }

        public ImmutableArray<IFsNodeElement<IFsNode>> Content { get; }
    }
}
