using System.Collections.Immutable;
using Core.FileSystem;

namespace Core.CoreEngine
{
    public interface IEnginePerformer
    {
        IDirectoryElement DirectoryElement { get; }
        void PerformAllOperations();
        void PerformOperations(ImmutableArray<string> operations);
    }
}
