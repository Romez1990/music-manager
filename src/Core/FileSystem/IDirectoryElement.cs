using System;
using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectoryElement : IFsNodeElement<IDirectoryElement>
    {
        ImmutableArray<IFsNodeElement<object>> Content { get; }
        IDirectoryElement SelectContent(Func<IFsNodeElement<object>, IFsNodeElement<object>> selector);
        IDirectoryElement SelectContent(Func<IFsNodeElement<object>, int, IFsNodeElement<object>> selector);
    }
}
