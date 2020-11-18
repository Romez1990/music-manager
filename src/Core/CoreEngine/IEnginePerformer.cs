using System.Collections.Immutable;
using Core.FileSystem;
using Core.Operations.Operation;

namespace Core.CoreEngine
{
    public interface IEnginePerformer
    {
        IDirectoryElement DirectoryElement { get; }
        OperationResult PerformOperations(ImmutableArray<string> operations);
    }
}
