using System.Collections.Immutable;
using Core.FileSystem;
using Core.Operations;

namespace Core.CoreEngine
{
    public interface IEnginePerformer
    {
        IDirectoryElement Directory { get; }
        OperationResult PerformOperations(ImmutableArray<string> operations);
    }
}
