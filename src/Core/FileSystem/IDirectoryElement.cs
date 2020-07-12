using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectoryElement : IFsNodeElement
    {
        ImmutableArray<IFsNodeElement> Content { get; }
    }
}
