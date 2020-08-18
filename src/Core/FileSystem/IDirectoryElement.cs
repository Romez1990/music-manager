using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectoryElement : IFsNodeElement<IDirectoryElement>
    {
        ImmutableArray<IFsNodeElement<object>> Content { get; }
    }
}
