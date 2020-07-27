using System.Collections.Immutable;
using Core.FileSystem;

namespace Core.CoreEngine
{
    public interface IEnginePerformer
    {
        IDirectoryElement DirectoryElement { get; }
        IDirectoryElement PerformOperations(ImmutableArray<string> operations);
    }
}
