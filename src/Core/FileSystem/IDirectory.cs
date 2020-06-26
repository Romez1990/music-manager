using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectory : IFsNode
    {
        IDirectory Parent { get; }

        ImmutableArray<IFsNode> Content { get; }
    }
}
