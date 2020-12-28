using System;
using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectoryElement : IFsNodeElement<IDirectoryElement>
    {
        ImmutableArray<IFsNodeElement<object>> Content { get; }
        IDirectoryElement MapContent(Func<IFsNodeElement<object>, IFsNodeElement<object>> function);
        IDirectoryElement MapContent(Func<int, IFsNodeElement<object>, IFsNodeElement<object>> function);
    }
}
