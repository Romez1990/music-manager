using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectory : IFsNode<IDirectory>
    {
        ImmutableArray<IFsNode<object>> Content { get; }
    }
}
