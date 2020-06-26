using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectoryElement : IFsNodeElement
    {
        new IDirectory FsNode { get; }

        ImmutableArray<IFsNodeElement> Content { get; }
    }
}
