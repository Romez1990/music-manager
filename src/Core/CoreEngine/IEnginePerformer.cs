using System.Collections.Generic;
using Core.FileSystem;
using Core.Operation;

namespace Core.CoreEngine
{
    public interface IEnginePerformer
    {
        IDirectoryElement DirectoryElement { get; }
        void PerformOperations(IEnumerable<IOperation> operations);
    }
}
